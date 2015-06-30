#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Target::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_TARGET_WINDOW));
		Image		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_TARGET_IMAGE));
		Name		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_TARGET_NAME));
		Layout		= static_cast<CEGUI::HorizontalLayoutContainer*>(Window->getChild(UI_NAME_TARGET_LAYOUT));
		Inspect		= static_cast<CEGUI::PushButton*>(Layout->getChild(UI_NAME_TARGET_INSPECT));
		Attack		= static_cast<CEGUI::PushButton*>(Layout->getChild(UI_NAME_TARGET_ATTACK));
		Activate	= static_cast<CEGUI::PushButton*>(Layout->getChild(UI_NAME_TARGET_ACTIVATE));
		Buy			= static_cast<CEGUI::PushButton*>(Layout->getChild(UI_NAME_TARGET_BUY));
		Trade		= static_cast<CEGUI::PushButton*>(Layout->getChild(UI_NAME_TARGET_TRADE));
		Loot		= static_cast<CEGUI::PushButton*>(Layout->getChild(UI_NAME_TARGET_LOOT));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutTarget->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutTarget->getSize());

		// attach listener to Data
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);

		// image composer for head picture (hotspot=1 is head)
		imageComposer = gcnew ImageComposerCEGUI<ObjectBase^>();
		imageComposer->ApplyYOffset = false;
        imageComposer->HotspotIndex = 0;
        imageComposer->IsScalePow2 = false;
        imageComposer->UseViewerFrame = false;
		imageComposer->Width = (unsigned int)Image->getPixelSize().d_width;
        imageComposer->Height = (unsigned int)Image->getPixelSize().d_height;
        imageComposer->CenterHorizontal = true;
        imageComposer->CenterVertical = true;
		imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewImageAvailable);
	
		// subscribe clicks to actions
		Inspect->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Target::OnInspectMouseClick));
		Attack->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Target::OnAttackMouseClick));
		Activate->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Target::OnActivateMouseClick));
		Buy->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Target::OnBuyMouseClick));
		Trade->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Target::OnTradeMouseClick));
		Loot->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Target::OnLootMouseClick));

		// subscribe mouse events
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::Target::OnMouseDown));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::Target::OnMouseUp));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::Window::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Target::OnKeyUp));
	};

	void ControllerUI::Target::Destroy()
	{	 
		// detach listener from Data
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
        
		// detach targetobject listener
		if (targetObject != nullptr)
			targetObject->PropertyChanged -= gcnew PropertyChangedEventHandler(OnTargetObjectPropertyChanged);
			
		imageComposer->NewImageAvailable -= gcnew ::System::EventHandler(OnNewImageAvailable);			
	};

	void ControllerUI::Target::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// targetobject
		if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_TARGETOBJECT))
		{
			// detach old listener
			if (targetObject != nullptr)
				targetObject->PropertyChanged -= gcnew PropertyChangedEventHandler(OnTargetObjectPropertyChanged);
			
			// save reference
			targetObject = OgreClient::Singleton->Data->TargetObject;

			// possibly set to null
			imageComposer->DataSource = targetObject;

			if (targetObject != nullptr)
			{							
				// attach listener
				targetObject->PropertyChanged += gcnew PropertyChangedEventHandler(OnTargetObjectPropertyChanged);
			
				// get color
				::CEGUI::Colour color = ::CEGUI::Colour(
					NameColors::GetColorFor(targetObject->Flags));
				
				// set color
				Name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				Image->setTooltipText(StringConvert::CLRToCEGUI(targetObject->Name));

				// no name and image for invis objects except ourself
				if (targetObject->ID != OgreClient::Singleton->Data->AvatarID &&
					targetObject->Flags->Drawing == ObjectFlags::DrawingType::Invisible)
				{
					Name->setText(STRINGEMPTY);
					Image->setVisible(false);
				}
				else
				{
					Name->setText(StringConvert::CLRToCEGUI(targetObject->Name));					
					Image->setVisible(true);
				}
				
				// set button availability
				Attack->setEnabled(targetObject->Flags->IsAttackable);
				Activate->setEnabled(targetObject->Flags->IsActivatable || targetObject->Flags->IsContainer);
				Buy->setEnabled(targetObject->Flags->IsBuyable);
				Trade->setEnabled(targetObject->Flags->IsOfferable);
				Loot->setEnabled(targetObject->Flags->IsGettable);

				// show target window but don't give it input focus
				Window->show();
				Window->moveToFront();
				Window->deactivate();
			}
			else
			{
				// unset
				Image->setProperty(UI_PROPNAME_NORMALIMAGE, STRINGEMPTY);
				Image->setProperty(UI_PROPNAME_HOVERIMAGE, STRINGEMPTY);
				Image->setProperty(UI_PROPNAME_PUSHEDIMAGE, STRINGEMPTY);
				Image->setTooltipText(STRINGEMPTY);
				Name->setText(STRINGEMPTY);

				// hide window
				Window->hide();
			}
		}
	};
	
	void ControllerUI::Target::OnTargetObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// flags or name
		if (::System::String::Equals(e->PropertyName, ObjectBase::PROPNAME_FLAGS) ||
			::System::String::Equals(e->PropertyName, ObjectBase::PROPNAME_NAME))
		{
			// get color
			::CEGUI::Colour color = ::CEGUI::Colour(
				NameColors::GetColorFor(targetObject->Flags));
				
			// set color
			Name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
			Image->setTooltipText(StringConvert::CLRToCEGUI(targetObject->Name));

			// no name and image for invis objects except ourself
			if (targetObject->ID != OgreClient::Singleton->Data->AvatarID && 
				targetObject->Flags->Drawing == ObjectFlags::DrawingType::Invisible)
			{
				Name->setText(STRINGEMPTY);
				Image->setVisible(false);
			}
			else
			{
				Name->setText(StringConvert::CLRToCEGUI(targetObject->Name));
				Image->setVisible(true);			
			}
				
			// set button availability
			Attack->setEnabled(targetObject->Flags->IsAttackable);
			Activate->setEnabled(targetObject->Flags->IsActivatable || targetObject->Flags->IsContainer);
			Buy->setEnabled(targetObject->Flags->IsBuyable);
			Trade->setEnabled(targetObject->Flags->IsOfferable);
			Loot->setEnabled(targetObject->Flags->IsGettable);
		}
	};

	void ControllerUI::Target::OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		Image->setProperty(UI_PROPNAME_NORMALIMAGE, *imageComposer->Image->TextureName);
		Image->setProperty(UI_PROPNAME_HOVERIMAGE, *imageComposer->Image->TextureName);
		Image->setProperty(UI_PROPNAME_PUSHEDIMAGE, *imageComposer->Image->TextureName);
	};

	bool UICallbacks::Target::OnInspectMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// request look of current target
		OgreClient::Singleton->SendReqLookMessage();
		
		return true;
	};

	bool UICallbacks::Target::OnAttackMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// request attack of current target
		OgreClient::Singleton->SendReqAttackMessage();
		
		return true;
	};

	bool UICallbacks::Target::OnActivateMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// request activate of current target
		OgreClient::Singleton->ExecAction(AvatarAction::Activate);

		return true;
	};

	bool UICallbacks::Target::OnBuyMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// request to buy from current target
		OgreClient::Singleton->SendReqBuyMessage();
		
		return true;
	};

	bool UICallbacks::Target::OnTradeMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// show trade panel
		OgreClient::Singleton->ExecAction(AvatarAction::Trade);

		return true;
	};

	bool UICallbacks::Target::OnLootMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// request to buy from current target
		OgreClient::Singleton->SendReqGetMessage();
		
		return true;
	};

	bool UICallbacks::Target::OnMouseDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// set this window as moving one
		ControllerUI::MovingWindow = ControllerUI::Target::Window;

		return true;
	};

	bool UICallbacks::Target::OnMouseUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// unset this window as moving one
		ControllerUI::MovingWindow = nullptr;

		return true;
	};

	bool UICallbacks::Target::OnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			OgreClient::Singleton->Data->TargetID = 0xFFFFFFF;

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}
};};
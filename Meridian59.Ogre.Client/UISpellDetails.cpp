#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::SpellDetails::Initialize()
	{
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_SPELLDETAILS_WINDOW));
		Image		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SPELLDETAILS_IMAGE));
		Name		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SPELLDETAILS_NAME));
		SchoolName = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SPELLDETAILS_SCHOOLNAME));
		SpellLevel = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SPELLDETAILS_SPELLLEVEL));
		ManaCost = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SPELLDETAILS_MANACOST));
		VigorCost = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SPELLDETAILS_VIGORCOST));
		Description = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_SPELLDETAILS_DESCRIPTION));

		// image composer for picture
		imageComposer = gcnew ImageComposerCEGUI<ObjectBase^>();
		imageComposer->ApplyYOffset = false;
		imageComposer->IsScalePow2 = false;
		imageComposer->UseViewerFrame = true;
		imageComposer->Width = (unsigned int)Image->getPixelSize().d_width;
		imageComposer->Height = (unsigned int)Image->getPixelSize().d_height;
		imageComposer->CenterHorizontal = true;
		imageComposer->CenterVertical = true;
		imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewImageAvailable);

		// attach listener to LookSpell
		OgreClient::Singleton->Data->LookSpell->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnLookSpellObjectPropertyChanged);

		// subscribe mouse wheel to image
		Image->subscribeEvent(CEGUI::Window::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::SpellDetails::OnImageMouseWheel));
		Image->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::SpellDetails::OnImageMouseClick));

		// subscribe keydown on description box
		Description->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::SpellDetails::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::SpellDetails::OnWindowKeyUp));
	};

	void ControllerUI::SpellDetails::Destroy()
	{	 
		// detach listener to LookSpell
		OgreClient::Singleton->Data->LookSpell->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnLookSpellObjectPropertyChanged);

		// detach image listener
		imageComposer->NewImageAvailable -=
			gcnew ::System::EventHandler(OnNewImageAvailable);
	};

	void ControllerUI::SpellDetails::ApplyLanguage()
	{
	};

	void ControllerUI::SpellDetails::OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e)
	{
		Image->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
	};

	void ControllerUI::SpellDetails::OnLookSpellObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// objectbase
		if (::System::String::Equals(e->PropertyName, SpellInfo::PROPNAME_OBJECTBASE))
		{
			ObjectBase^ LookSpell = OgreClient::Singleton->Data->LookSpell->ObjectBase;
			SpellInfo^ LookInfo = OgreClient::Singleton->Data->LookSpell;

			// unset
			Image->setProperty(UI_PROPNAME_IMAGE, STRINGEMPTY);
			Name->setText(STRINGEMPTY);
			SchoolName->setText(STRINGEMPTY);
			SpellLevel->setText(STRINGEMPTY);
			ManaCost->setText(STRINGEMPTY);
			VigorCost->setText(STRINGEMPTY);

			// possibly set to null
			imageComposer->DataSource = LookSpell;

			if (LookSpell != nullptr)
			{
				// get color
				::CEGUI::Colour color = ::CEGUI::Colour(
					NameColors::GetColorFor(LookSpell->Flags));

				// set color and name
				Name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				Name->setText(StringConvert::CLRToCEGUI(LookSpell->Name));
				SchoolName->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				SchoolName->setText(StringConvert::CLRToCEGUI(LookInfo->SchoolName->FullString));
				SpellLevel->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				SpellLevel->setText(StringConvert::CLRToCEGUI(LookInfo->SpellLevel->FullString));
				ManaCost->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				ManaCost->setText(StringConvert::CLRToCEGUI(LookInfo->ManaCost->FullString));
				VigorCost->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				VigorCost->setText(StringConvert::CLRToCEGUI(LookInfo->VigorCost->FullString));
			}
		}

		// description
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_MESSAGE))
		{
			ServerString^ text = OgreClient::Singleton->Data->LookSpell->Message;

			if (text != nullptr)
				Description->setText(StringConvert::CLRToCEGUI(text->FullString));
		}

		// isvisible
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_ISVISIBLE))
		{
			// set window visibility
			Window->setVisible(OgreClient::Singleton->Data->LookSpell->IsVisible);
			Window->moveToFront();
		}
	};

	bool UICallbacks::SpellDetails::OnImageMouseWheel(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		ObjectBase^ LookSpell = OgreClient::Singleton->Data->LookSpell->ObjectBase;

		if (LookSpell != nullptr)
			LookSpell->ViewerAngle += (unsigned short)(args.wheelChange * 200.0f);

		return true;
	};

	bool UICallbacks::SpellDetails::OnImageMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		ObjectBase^ LookSpell = OgreClient::Singleton->Data->LookSpell->ObjectBase;

		if (LookSpell == nullptr)
			return true;

		// increment angle by this (each click is a new frame for 6 frames-groups)
		unsigned short increment = GeometryConstants::MAXANGLE / 6;

		// flip direction for right button
		if (args.button == CEGUI::MouseButton::RightButton)
			increment = -increment;

		// rotate
		LookSpell->ViewerAngle += increment;

		return true;
	};

	bool UICallbacks::SpellDetails::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// clear (view will react)
			OgreClient::Singleton->Data->LookSpell->IsVisible = false;

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}

	bool UICallbacks::SpellDetails::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// set not visible in datalayer (view will react)
		OgreClient::Singleton->Data->LookSpell->IsVisible = false;
		
		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}
};};
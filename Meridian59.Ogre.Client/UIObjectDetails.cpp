#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::ObjectDetails::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_OBJECTDETAILS_WINDOW));
		Image		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_OBJECTDETAILS_IMAGE));
		Name		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_OBJECTDETAILS_NAME));
		Description = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_OBJECTDETAILS_DESCRIPTION));
		Inscription = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_OBJECTDETAILS_INSCRIPTION));

		// set initial layout type
		SetLayout(OgreClient::Singleton->Data->LookObject->LookType);

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
	
		// attach listener to lookobject
		OgreClient::Singleton->Data->LookObject->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnLookObjectPropertyChanged);
        
		// subscribe mouse wheel to image
		Image->subscribeEvent(CEGUI::Window::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::ObjectDetails::OnImageMouseWheel));
		
		// subscribe keydown on description box and inscription
		Description->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		Inscription->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::ObjectDetails::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::ObjectDetails::OnWindowKeyUp));
	};

	void ControllerUI::ObjectDetails::Destroy()
	{	 
		// detach listener to lookobject
		OgreClient::Singleton->Data->LookObject->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnLookObjectPropertyChanged);

		// detach image listener
		imageComposer->NewImageAvailable -=
			gcnew ::System::EventHandler(OnNewImageAvailable);      		
	};

	void ControllerUI::ObjectDetails::OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		Image->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
	};

	void ControllerUI::ObjectDetails::OnLookObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// objectbase
		if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_OBJECTBASE))
		{
			ObjectBase^ lookObject = OgreClient::Singleton->Data->LookObject->ObjectBase;
			
			// unset
			Image->setProperty(UI_PROPNAME_IMAGE, STRINGEMPTY);
			Name->setText(STRINGEMPTY);
						
			// possibly set to null
			imageComposer->DataSource = lookObject;	

			if (lookObject != nullptr)
			{
				// get color
				::CEGUI::Colour color = ::CEGUI::Colour(
					NameColors::GetColorFor(lookObject->Flags));
		
				// set color and name
				Name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				Name->setText(StringConvert::CLRToCEGUI(lookObject->Name));
			}	
		}

		// description
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_MESSAGE))
		{
			ServerString^ text = OgreClient::Singleton->Data->LookObject->Message;

			if (text != nullptr)
				Description->setText(StringConvert::CLRToCEGUI(text->FullString));
		}

		// inscription
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_INSCRIPTION))
		{
			ServerString^ text = OgreClient::Singleton->Data->LookObject->Inscription;

			if (text != nullptr)
				Inscription->setText(StringConvert::CLRToCEGUI(text->FullString));
		}

		// looktype
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_LOOKTYPE))
		{
			LookTypeFlags^ type = OgreClient::Singleton->Data->LookObject->LookType;

			// set layout
			SetLayout(type);
		}

		// isvisible
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_ISVISIBLE))
		{
			// set window visibility
			Window->setVisible(OgreClient::Singleton->Data->LookObject->IsVisible);
			Window->moveToFront();
		}
	};

	void ControllerUI::ObjectDetails::SetLayout(LookTypeFlags^ LayoutType)
	{
		// some positioning
		CEGUI::UVector2 posImage = Image->getPosition();
		CEGUI::UVector2 posName = Name->getPosition();
		CEGUI::Sizef sizeImage = Image->getPixelSize();
		CEGUI::Sizef sizeName = Name->getPixelSize();
			
		float val1 = posImage.d_x.d_offset + sizeImage.d_width + (float)UI_DEFAULTPADDING;
		float val2 = posName.d_y.d_offset + sizeName.d_height + (float)UI_DEFAULTPADDING;

		// no inscription
		if (!LayoutType->IsInscribed && !LayoutType->IsEditable)
		{
			Inscription->setReadOnly(true);
			Inscription->setVisible(false);

			Description->setArea(
				CEGUI::UDim(0, val1),
				CEGUI::UDim(0, val2),
				CEGUI::UDim(1.0f, -val1 - (float)UI_DEFAULTPADDING),
				CEGUI::UDim(1.0f, -val2 - (float)UI_DEFAULTPADDING));
										
			Window->setHeight(CEGUI::UDim(0, 221.0f));	
		}

		// non editable inscription
		else if (LayoutType->IsInscribed && !LayoutType->IsEditable)
		{
			Inscription->setReadOnly(true);
			Inscription->setVisible(true);

			Description->setArea(
				CEGUI::UDim(0, val1),
				CEGUI::UDim(0, val2),
				CEGUI::UDim(1.0f, -val1 - (float)UI_DEFAULTPADDING),
				CEGUI::UDim(0, sizeImage.d_height - sizeName.d_height - (float)UI_DEFAULTPADDING));
						
			Window->setHeight(CEGUI::UDim(0, 512.0f));
		}

		// editable inscription
		else
		{
			Inscription->setReadOnly(false);
			Inscription->setVisible(true);

			Description->setArea(
				CEGUI::UDim(0, val1),
				CEGUI::UDim(0, val2),
				CEGUI::UDim(1.0f, -val1 - (float)UI_DEFAULTPADDING),
				CEGUI::UDim(0, sizeImage.d_height - sizeName.d_height - (float)UI_DEFAULTPADDING));

			Window->setHeight(CEGUI::UDim(0, 512.0f));
		}
	};

	bool UICallbacks::ObjectDetails::OnImageMouseWheel(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		ObjectBase^ lookObject = OgreClient::Singleton->Data->LookObject->ObjectBase;
		
		if (lookObject != nullptr)
			lookObject->ViewerAngle += (unsigned short)(args.wheelChange * 200.0f);
		
		return true;
	}

	bool UICallbacks::ObjectDetails::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// clear (view will react)
			OgreClient::Singleton->Data->LookObject->IsVisible = false;

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}

	bool UICallbacks::ObjectDetails::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// set not visible in datalayer (view will react)
		OgreClient::Singleton->Data->LookObject->IsVisible = false;
		
		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}
};};
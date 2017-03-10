#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::SkillDetails::Initialize()
	{
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_SKILLDETAILS_WINDOW));
		Image		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SKILLDETAILS_IMAGE));
		Name		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SKILLDETAILS_NAME));
		SchoolName = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SKILLDETAILS_SCHOOLNAME));
		SkillLevel = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_SKILLDETAILS_SKILLLEVEL));
		Description = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_SKILLDETAILS_DESCRIPTION));

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

		// attach listener to LookSkill
		OgreClient::Singleton->Data->LookSkill->PropertyChanged +=
			gcnew PropertyChangedEventHandler(OnLookSkillObjectPropertyChanged);

		// subscribe mouse wheel to image
		Image->subscribeEvent(CEGUI::Window::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::SkillDetails::OnImageMouseWheel));
		Image->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::SkillDetails::OnImageMouseClick));

		// subscribe keydown on description box
		Description->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::SkillDetails::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::SkillDetails::OnWindowKeyUp));
	};

	void ControllerUI::SkillDetails::Destroy()
	{
		// detach listener to LookSkill
		OgreClient::Singleton->Data->LookSkill->PropertyChanged -=
			gcnew PropertyChangedEventHandler(OnLookSkillObjectPropertyChanged);

		// detach image listener
		imageComposer->NewImageAvailable -=
			gcnew ::System::EventHandler(OnNewImageAvailable);
	};

	void ControllerUI::SkillDetails::ApplyLanguage()
	{
	};

	void ControllerUI::SkillDetails::OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e)
	{
		Image->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
	};

	void ControllerUI::SkillDetails::OnLookSkillObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// objectbase
		if (::System::String::Equals(e->PropertyName, SkillInfo::PROPNAME_OBJECTBASE))
		{
			ObjectBase^ LookSkill = OgreClient::Singleton->Data->LookSkill->ObjectBase;
			SkillInfo^ LookInfo = OgreClient::Singleton->Data->LookSkill;

			// unset
			Image->setProperty(UI_PROPNAME_IMAGE, STRINGEMPTY);
			Name->setText(STRINGEMPTY);
			SchoolName->setText(STRINGEMPTY);
			SkillLevel->setText(STRINGEMPTY);

			// possibly set to null
			imageComposer->DataSource = LookSkill;

			if (LookSkill != nullptr)
			{
				// get color
				::CEGUI::Colour color = ::CEGUI::Colour(
					NameColors::GetColorFor(LookSkill->Flags));

				// set color and name
				Name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				Name->setText(StringConvert::CLRToCEGUI(LookSkill->Name));
				SchoolName->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				SchoolName->setText(StringConvert::CLRToCEGUI(LookInfo->SchoolName->FullString));
				SkillLevel->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				SkillLevel->setText(StringConvert::CLRToCEGUI(LookInfo->SkillLevel->FullString));
			}
		}

		// description
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_MESSAGE))
		{
			ServerString^ text = OgreClient::Singleton->Data->LookSkill->Message;

			if (text != nullptr)
				Description->setText(StringConvert::CLRToCEGUI(text->FullString));
		}

		// isvisible
		else if (::System::String::Equals(e->PropertyName, ObjectInfo::PROPNAME_ISVISIBLE))
		{
			// set window visibility
			Window->setVisible(OgreClient::Singleton->Data->LookSkill->IsVisible);
			Window->moveToFront();
		}
	};

	bool UICallbacks::SkillDetails::OnImageMouseWheel(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		ObjectBase^ LookSkill = OgreClient::Singleton->Data->LookSkill->ObjectBase;

		if (LookSkill != nullptr)
			LookSkill->ViewerAngle += (unsigned short)(args.wheelChange * 200.0f);

		return true;
	};

	bool UICallbacks::SkillDetails::OnImageMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		ObjectBase^ LookSkill = OgreClient::Singleton->Data->LookSkill->ObjectBase;

		if (LookSkill == nullptr)
			return true;

		// increment angle by this (each click is a new frame for 6 frames-groups)
		unsigned short increment = GeometryConstants::MAXANGLE / 6;

		// flip direction for right button
		if (args.button == CEGUI::MouseButton::RightButton)
			increment = -increment;

		// rotate
		LookSkill->ViewerAngle += increment;

		return true;
	};

	bool UICallbacks::SkillDetails::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// clear (view will react)
			OgreClient::Singleton->Data->LookSkill->IsVisible = false;

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}

	bool UICallbacks::SkillDetails::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// set not visible in datalayer (view will react)
		OgreClient::Singleton->Data->LookSkill->IsVisible = false;
		
		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}
};};
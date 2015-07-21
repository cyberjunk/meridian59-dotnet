#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::AvatarCreateWizard::Initialize()
	{
		CharCreationInfo^ info = OgreClient::Singleton->Data->CharCreationInfo;

		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_AVATARCREATEWIZARD_WINDOW));
		TabControl		= static_cast<CEGUI::TabControl*>(Window->getChild(UI_NAME_AVATARCREATEWIZARD_TABCONTROL));
		
		TabBasic		= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_AVATARCREATEWIZARD_TABBASIC));
		TabAttributes	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_AVATARCREATEWIZARD_TABATTRIBUTES));
		TabSpellsSkills	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_AVATARCREATEWIZARD_TABSPELLSSKILLS));
		
		// Controls in TabBasic
		NameDesc		= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_NAMEDESC));
		Name			= static_cast<CEGUI::Editbox*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_NAME));
		Image			= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_IMAGE));
		GenderDesc		= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_GENDERDESC));
		GenderMale		= static_cast<CEGUI::RadioButton*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_GENDERMALE));
		GenderFemale	= static_cast<CEGUI::RadioButton*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_GENDERFEMALE));
		SkinColorDesc	= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_SKINCOLORDESC));
		SkinColor		= static_cast<CEGUI::Slider*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_SKINCOLOR));
		HairDesc		= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_HAIRDESC));
		Hair			= static_cast<CEGUI::Slider*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_HAIR));
		HairColorDesc	= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_HAIRCOLORDESC));
		HairColor		= static_cast<CEGUI::Slider*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_HAIRCOLOR));
		EyesDesc		= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_EYESDESC));
		Eyes			= static_cast<CEGUI::Slider*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_EYES));
		NoseDesc		= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_NOSEDESC));
		Nose			= static_cast<CEGUI::Slider*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_NOSE));
		MouthDesc		= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_MOUTHDESC));
		Mouth			= static_cast<CEGUI::Slider*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_MOUTH));
		DescriptionDesc	= static_cast<CEGUI::Window*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_DESCRIPTIONDESC));
		Description		= static_cast<CEGUI::MultiLineEditbox*>(TabBasic->getChild(UI_NAME_AVATARCREATEWIZARD_DESCRIPTION));
		
		// Controls in TabAttributes
		ProfilesDesc	= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_PROFILESDESC));
		Profiles		= static_cast<CEGUI::Combobox*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_PROFILES));
		MightDesc		= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_MIGHTDESC));
		Might			= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_MIGHT));
		IntellectDesc	= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_INTELLECTDESC));
		Intellect		= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_INTELLECT));
		StaminaDesc		= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_STAMINADESC));
		Stamina			= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_STAMINA));
		AgilityDesc		= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_AGILITYDESC));
		Agility			= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_AGILITY));
		MysticismDesc	= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_MYSTICISMDESC));
		Mysticism		= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_MYSTICISM));
		AimDesc			= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_AIMDESC));
		Aim				= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_AIM));
		AttributesAvailableDesc		= static_cast<CEGUI::Window*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_ATTRIBUTESAVAILABLEDESC));
		AttributesAvailable			= static_cast<CEGUI::ProgressBar*>(TabAttributes->getChild(UI_NAME_AVATARCREATEWIZARD_ATTRIBUTESAVAILABLE));
		
		// Controls in TabSpellsSkills
		SpellsDesc		= static_cast<CEGUI::Window*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SPELLSDESC));
		Spells			= static_cast<CEGUI::ItemListbox*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SPELLS));
		SkillsDesc		= static_cast<CEGUI::Window*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SKILLSDESC));
		Skills			= static_cast<CEGUI::ItemListbox*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SKILLS));
		SelectedSpellsDesc		= static_cast<CEGUI::Window*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SELECTEDSPELLSDESC));
		SelectedSpells			= static_cast<CEGUI::ItemListbox*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SELECTEDSPELLS));
		SelectedSkillsDesc		= static_cast<CEGUI::Window*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SELECTEDSKILLSDESC));
		SelectedSkills			= static_cast<CEGUI::ItemListbox*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SELECTEDSKILLS));
		SkillPointsAvailable	= static_cast<CEGUI::ProgressBar*>(TabSpellsSkills->getChild(UI_NAME_AVATARCREATEWIZARD_SKILLPOINTSAVAILABLE));
		
		// Buttons at the bottom
		ButtonBack = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_AVATARCREATEWIZARD_BUTTONBACK));
		ButtonNext = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_AVATARCREATEWIZARD_BUTTONNEXT));
		DataOK = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_AVATARCREATEWIZARD_DATAOK));

		// value of sliders is an index, so only integer like float steps
		SkinColor->setClickStep(1.0f);
		HairColor->setClickStep(1.0f);
		Hair->setClickStep(1.0f);
		Eyes->setClickStep(1.0f);			
		Nose->setClickStep(1.0f);
		Mouth->setClickStep(1.0f);

		// image composer for head picture (hotspot=1 is head)
		imageComposerHead = gcnew ImageComposerCEGUI<ObjectBase^>();
		imageComposerHead->ApplyYOffset = false;
		imageComposerHead->HotspotIndex = (unsigned char)KnownHotspot::HEAD;
        imageComposerHead->IsScalePow2 = false;
        imageComposerHead->UseViewerFrame = false;
		imageComposerHead->Width = (unsigned int)Image->getPixelSize().d_width;
        imageComposerHead->Height = (unsigned int)Image->getPixelSize().d_height;
        imageComposerHead->CenterHorizontal = true;
        imageComposerHead->CenterVertical = true;
		imageComposerHead->NewImageAvailable += gcnew ::System::EventHandler(OnNewHeadImageAvailable);
		imageComposerHead->DataSource = info->ExampleModel;

		// Profiles for attributes
		Profiles->addItem(new::CEGUI::ListboxTextItem(UI_AVATARCREATEWIZARD_PROFILE_CUSTOM));
		Profiles->addItem(new::CEGUI::ListboxTextItem(UI_AVATARCREATEWIZARD_PROFILE_WARRIOR));
		Profiles->addItem(new::CEGUI::ListboxTextItem(UI_AVATARCREATEWIZARD_PROFILE_MAGE));
		Profiles->addItem(new::CEGUI::ListboxTextItem(UI_AVATARCREATEWIZARD_PROFILE_HYBRID));
		Profiles->setText(UI_AVATARCREATEWIZARD_PROFILE_CUSTOM);
		Profiles->selectListItemWithEditboxText();

		// initial attributes		
		Might->setProgress((float)info->Might / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
		Might->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->Might) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		Intellect->setProgress((float)info->Intellect / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
		Intellect->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->Intellect) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		Stamina->setProgress((float)info->Stamina / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
		Stamina->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->Stamina) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		Agility->setProgress((float)info->Agility / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
		Agility->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->Agility) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		Mysticism->setProgress((float)info->Mysticism / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
		Mysticism->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->Mysticism) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		Aim->setProgress((float)info->Aim / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
		Aim->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->Aim) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		AttributesAvailable->setProgress((float)info->AttributesAvailable / (float)CharCreationInfo::ATTRIBUTE_MAXSUM);
		AttributesAvailable->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->AttributesAvailable) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXSUM));
		
		SkillPointsAvailable->setProgress((float)info->SkillPointsAvailable / (float)CharCreationInfo::SKILLPOINTS_MAXSUM);
		SkillPointsAvailable->setText(CEGUI::PropertyHelper<unsigned int>::toString(info->SkillPointsAvailable) + "/" + CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::SKILLPOINTS_MAXSUM));

		// attach listener
		OgreClient::Singleton->Data->CharCreationInfo->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnCharCreationInfoPropertyChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->Spells->ListChanged += 
			gcnew ListChangedEventHandler(OnSpellsListChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->Skills->ListChanged += 
			gcnew ListChangedEventHandler(OnSkillsListChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->SelectedSpells->ListChanged += 
			gcnew ListChangedEventHandler(OnSelectedSpellsListChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->SelectedSkills->ListChanged += 
			gcnew ListChangedEventHandler(OnSelectedSkillsListChanged);
        
		// subscribe tabchange event
		TabControl->subscribeEvent(CEGUI::TabControl::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnTabChanged));

		// subscribe gender radio button change
		GenderMale->subscribeEvent(CEGUI::RadioButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnGenderChanged));
		
		// subscribe mouse wheel to image
		Image->subscribeEvent(CEGUI::Window::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnImageMouseWheel));

		// subscribe sliders
		SkinColor->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnFaceSettingChanged));
		HairColor->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnFaceSettingChanged));
		Hair->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnFaceSettingChanged));
		Eyes->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnFaceSettingChanged));
		Nose->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnFaceSettingChanged));
		Mouth->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnFaceSettingChanged));

		// subscribe profilesbox
		Profiles->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnProfileChanged));

		// subscribe attributes
		Might->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Might->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Intellect->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Intellect->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Stamina->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Stamina->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Agility->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Agility->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Mysticism->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Mysticism->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Aim->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));
		Aim->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick));

		// subscribe keydown on name and description box
		Name->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		Description->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		
		// subscribe back/next button
		ButtonBack->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnButtonBackClicked));
		ButtonNext->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnButtonNextClicked));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnWindowKeyUp));
	};

	void ControllerUI::AvatarCreateWizard::Destroy()
	{	
		imageComposerHead->NewImageAvailable -= 
			gcnew ::System::EventHandler(OnNewHeadImageAvailable);
		
		OgreClient::Singleton->Data->CharCreationInfo->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnCharCreationInfoPropertyChanged);

		OgreClient::Singleton->Data->CharCreationInfo->Spells->ListChanged -= 
			gcnew ListChangedEventHandler(OnSpellsListChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->Skills->ListChanged -= 
			gcnew ListChangedEventHandler(OnSkillsListChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->SelectedSpells->ListChanged -= 
			gcnew ListChangedEventHandler(OnSelectedSpellsListChanged);
        
		OgreClient::Singleton->Data->CharCreationInfo->SelectedSkills->ListChanged -= 
			gcnew ListChangedEventHandler(OnSelectedSkillsListChanged);
        
	};

	void ControllerUI::AvatarCreateWizard::OnCharCreationInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		CharCreationInfo^ creationInfo = OgreClient::Singleton->Data->CharCreationInfo;

		/// APPEARANCE STUFF

		// haircolors
		if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_HAIRCOLORS))
		{
			HairColor->setMaxValue((float)::System::Math::Max(creationInfo->HairColors->Length - 1, 0));
		}

		// skincolors
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_SKINCOLORS))
		{
			SkinColor->setMaxValue((float)::System::Math::Max(creationInfo->SkinColors->Length - 1, 0));
		}

		// hairids (male/female)
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_MALEHAIRIDS) &&
			GenderMale->isSelected())
		{
			Hair->setMaxValue((float)::System::Math::Max(creationInfo->MaleHairIDs->Length - 1, 0));
		}
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_FEMALEHAIRIDS) &&
			!GenderMale->isSelected())
		{
			Hair->setMaxValue((float)::System::Math::Max(creationInfo->FemaleHairIDs->Length - 1, 0));
		}

		// eyeids (male/female)
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_MALEEYEIDS) &&
			GenderMale->isSelected())
		{
			Eyes->setMaxValue((float)::System::Math::Max(creationInfo->MaleEyeIDs->Length - 1, 0));
		}
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_FEMALEEYEIDS) &&
			!GenderMale->isSelected())
		{
			Eyes->setMaxValue((float)::System::Math::Max(creationInfo->FemaleEyeIDs->Length - 1, 0));
		}

		// noseids (male/female)
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_MALENOSEIDS) &&
			GenderMale->isSelected())
		{
			Nose->setMaxValue((float)::System::Math::Max(creationInfo->MaleNoseIDs->Length - 1, 0));
		}
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_FEMALENOSEIDS) &&
			!GenderMale->isSelected())
		{
			Nose->setMaxValue((float)::System::Math::Max(creationInfo->FemaleNoseIDs->Length - 1, 0));
		}

		// mouthids (male/female)
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_MALEMOUTHIDS) &&
			GenderMale->isSelected())
		{
			Mouth->setMaxValue((float)::System::Math::Max(creationInfo->MaleMouthIDs->Length - 1, 0));
		}
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_FEMALEMOUTHIDS) &&
			!GenderMale->isSelected())
		{
			Mouth->setMaxValue((float)::System::Math::Max(creationInfo->FemaleMouthIDs->Length - 1, 0));
		}

		// name
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_AVATARNAME))
		{
			Name->setText(StringConvert::CLRToCEGUI(creationInfo->AvatarName));
		}

		// description
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_AVATARDESCRIPTION))
		{
			Description->setText(StringConvert::CLRToCEGUI(creationInfo->AvatarDescription));
		}

		/// ATTRIBUTES

		// might
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_MIGHT))
		{
			Might->setProgress((float)creationInfo->Might / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
			
			Might->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->Might) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		}

		// intellect
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_INTELLECT))
		{
			Intellect->setProgress((float)creationInfo->Intellect / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
			
			Intellect->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->Intellect) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		}

		// stamina
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_STAMINA))
		{
			Stamina->setProgress((float)creationInfo->Stamina / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
			
			Stamina->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->Stamina) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		}

		// agility
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_AGILITY))
		{
			Agility->setProgress((float)creationInfo->Agility / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
			
			Agility->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->Agility) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		}

		// mysticism
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_MYSTICISM))
		{
			Mysticism->setProgress((float)creationInfo->Mysticism / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
			
			Mysticism->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->Mysticism) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		}

		// aim
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_AIM))
		{
			Aim->setProgress((float)creationInfo->Aim / (float)CharCreationInfo::ATTRIBUTE_MAXVALUE);
			
			Aim->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->Aim) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXVALUE));
		}

		// attributesavailable
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_ATTRIBUTESAVAILABLE))
		{
			AttributesAvailable->setProgress((float)creationInfo->AttributesAvailable / (float)CharCreationInfo::ATTRIBUTE_MAXSUM);
			
			AttributesAvailable->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->AttributesAvailable) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::ATTRIBUTE_MAXSUM));
		}

		/// SPELLS/SKILLS

		// attributesavailable
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_SKILLPOINTSAVAILABLE))
		{
			SkillPointsAvailable->setProgress((float)creationInfo->SkillPointsAvailable / (float)CharCreationInfo::SKILLPOINTS_MAXSUM);
			
			SkillPointsAvailable->setText(
				CEGUI::PropertyHelper<unsigned int>::toString(creationInfo->SkillPointsAvailable) + "/" + 
				CEGUI::PropertyHelper<unsigned int>::toString(CharCreationInfo::SKILLPOINTS_MAXSUM));
		}

		/// NOT OK FLAG
		else if (::System::String::Equals(e->PropertyName, CharCreationInfo::PROPNAME_ISDATAOK))
		{
			if (creationInfo->IsDataOK)
			{
				DataOK->setVisible(false);
			}
			else
			{
				DataOK->setVisible(true);
			}
		}
	};

	void ControllerUI::AvatarCreateWizard::OnNewHeadImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		Image->setProperty(UI_PROPNAME_IMAGE, *imageComposerHead->Image->TextureName);
	};

	void ControllerUI::AvatarCreateWizard::OnSpellsListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				SpellAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				SpellRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				//SpellChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::AvatarCreateWizard::OnSkillsListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				SkillAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				SkillRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				//SkillChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::AvatarCreateWizard::OnSelectedSpellsListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				SelectedSpellAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				SelectedSpellRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				//SelectedSpellChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::AvatarCreateWizard::OnSelectedSkillsListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				SelectedSkillAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				SelectedSkillRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				//SelectedSkillChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::AvatarCreateWizard::SpellAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		AvatarCreatorSpellObject^ obj = OgreClient::Singleton->Data->CharCreationInfo->Spells[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARCREATORSPELLOBJECTLISTBOXITEM);
		
		// set ID
		widget->setID(obj->ExtraID);

		// mousecursor
		widget->setTooltipText(StringConvert::CLRToCEGUI(obj->SpellDescription));

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseDoubleClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSpellDoubleClicked));
		
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSpellClicked));
		
		if (widget->getChildCount() > 2)
		{
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SPELLS_NAME);
			CEGUI::Window* school	= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SPELLS_SCHOOL);
			CEGUI::Window* points	= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SPELLS_POINTS);
			
			name->setText(StringConvert::CLRToCEGUI(obj->SpellName));
			school->setText(StringConvert::CLRToCEGUI(obj->SchoolType.ToString()));
			points->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SpellCost));
		}

		// insert widget in ui-list
		if ((int)Spells->getItemCount() > Index)
			Spells->insertItem(widget, Spells->getItemFromIndex(Index));
		
		// or add
		else
			Spells->addItem(widget);		
	};

	void ControllerUI::AvatarCreateWizard::SpellRemove(int Index)
	{
		if ((int)Spells->getItemCount() > Index)		
			Spells->removeItem(Spells->getItemFromIndex(Index));
	};

	void ControllerUI::AvatarCreateWizard::SkillAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		AvatarCreatorSkillObject^ obj = OgreClient::Singleton->Data->CharCreationInfo->Skills[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARCREATORSKILLOBJECTLISTBOXITEM);
		
		// set ID
		widget->setID(obj->ExtraID);

		// mousecursor
		widget->setTooltipText(StringConvert::CLRToCEGUI(obj->SkillDescription));

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseDoubleClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSkillDoubleClicked));
		
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSkillClicked));
		
		if (widget->getChildCount() > 1)
		{
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SKILLS_NAME);
			CEGUI::Window* points	= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SKILLS_POINTS);
			
			name->setText(StringConvert::CLRToCEGUI(obj->SkillName));
			points->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SkillCost));
		}

		// insert widget in ui-list
		if ((int)Skills->getItemCount() > Index)
			Skills->insertItem(widget, Skills->getItemFromIndex(Index));
		
		// or add
		else
			Skills->addItem(widget);		
	};

	void ControllerUI::AvatarCreateWizard::SkillRemove(int Index)
	{
		if ((int)Skills->getItemCount() > Index)		
			Skills->removeItem(Skills->getItemFromIndex(Index));
	};

	void ControllerUI::AvatarCreateWizard::SelectedSpellAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		AvatarCreatorSpellObject^ obj = OgreClient::Singleton->Data->CharCreationInfo->SelectedSpells[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARCREATORSPELLOBJECTLISTBOXITEM);
		
		// set ID
		widget->setID(obj->ExtraID);

		// mousecursor
		widget->setTooltipText(StringConvert::CLRToCEGUI(obj->SpellDescription));

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseDoubleClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSelectedSpellDoubleClicked));
		
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSelectedSpellClicked));
		
		if (widget->getChildCount() > 2)
		{
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SPELLS_NAME);
			CEGUI::Window* school	= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SPELLS_SCHOOL);
			CEGUI::Window* points	= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SPELLS_POINTS);
			
			name->setText(StringConvert::CLRToCEGUI(obj->SpellName));
			school->setText(StringConvert::CLRToCEGUI(obj->SchoolType.ToString()));
			points->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SpellCost));
		}

		// insert widget in ui-list
		if ((int)SelectedSpells->getItemCount() > Index)
			SelectedSpells->insertItem(widget, SelectedSpells->getItemFromIndex(Index));
		
		// or add
		else
			SelectedSpells->addItem(widget);		
	};

	void ControllerUI::AvatarCreateWizard::SelectedSpellRemove(int Index)
	{
		if ((int)SelectedSpells->getItemCount() > Index)		
			SelectedSpells->removeItem(SelectedSpells->getItemFromIndex(Index));
	};

	void ControllerUI::AvatarCreateWizard::SelectedSkillAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		AvatarCreatorSkillObject^ obj = OgreClient::Singleton->Data->CharCreationInfo->SelectedSkills[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARCREATORSKILLOBJECTLISTBOXITEM);
		
		// set ID
		widget->setID(obj->ExtraID);

		// mousecursor
		widget->setTooltipText(StringConvert::CLRToCEGUI(obj->SkillDescription));

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseDoubleClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSelectedSkillDoubleClicked));
		
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::AvatarCreateWizard::OnSelectedSkillClicked));
		
		if (widget->getChildCount() > 1)
		{
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SKILLS_NAME);
			CEGUI::Window* points	= (CEGUI::Window*)widget->getChildAtIdx(UI_AVATARCREATOR_CHILDINDEX_SKILLS_POINTS);
			
			name->setText(StringConvert::CLRToCEGUI(obj->SkillName));
			points->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SkillCost));
		}

		// insert widget in ui-list
		if ((int)SelectedSkills->getItemCount() > Index)
			SelectedSkills->insertItem(widget, SelectedSkills->getItemFromIndex(Index));
		
		// or add
		else
			SelectedSkills->addItem(widget);		
	};

	void ControllerUI::AvatarCreateWizard::SelectedSkillRemove(int Index)
	{
		if ((int)SelectedSkills->getItemCount() > Index)		
			SelectedSkills->removeItem(SelectedSkills->getItemFromIndex(Index));
	};

	bool UICallbacks::AvatarCreateWizard::OnImageMouseWheel(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		ObjectBase^ headObject = OgreClient::Singleton->Data->CharCreationInfo->ExampleModel;
		
		if (headObject != nullptr)
			headObject->ViewerAngle += (unsigned short)(args.wheelChange * 200.0f);
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnTabChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
		CEGUI::TabControl* tabCtr			= ControllerUI::AvatarCreateWizard::TabControl;

		// get active tab index
		size_t activeTab = tabCtr->getSelectedTabIndex();
		
		switch(activeTab)
		{
			case 0:
				ControllerUI::AvatarCreateWizard::ButtonBack->setVisible(false);
				ControllerUI::AvatarCreateWizard::ButtonNext->setText("Next");
				break;

			case 1:
				ControllerUI::AvatarCreateWizard::ButtonBack->setVisible(true);
				ControllerUI::AvatarCreateWizard::ButtonNext->setText("Next");
				break;

			case 2:
				ControllerUI::AvatarCreateWizard::ButtonBack->setVisible(true);
				ControllerUI::AvatarCreateWizard::ButtonNext->setText("Finish");
				break;
		}
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnFaceSettingChanged(const CEGUI::EventArgs& e)
	{
		OgreClient::Singleton->Data->CharCreationInfo->SetExampleModel(
			ControllerUI::AvatarCreateWizard::GenderMale->isSelected() ? Gender::Male : Gender::Female,
			(int)ControllerUI::AvatarCreateWizard::SkinColor->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::HairColor->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Hair->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Eyes->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Nose->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Mouth->getCurrentValue());
			
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnGenderChanged(const CEGUI::EventArgs& e)
	{
		CharCreationInfo^ creationInfo = OgreClient::Singleton->Data->CharCreationInfo;

		CEGUI::Slider* hair = ControllerUI::AvatarCreateWizard::Hair;
		CEGUI::Slider* eyes = ControllerUI::AvatarCreateWizard::Eyes;
		CEGUI::Slider* nose = ControllerUI::AvatarCreateWizard::Nose;
		CEGUI::Slider* mouth = ControllerUI::AvatarCreateWizard::Mouth;

		// reset to 0
		hair->setCurrentValue(0.0f);
		eyes->setCurrentValue(0.0f);
		nose->setCurrentValue(0.0f);
		mouth->setCurrentValue(0.0f);

		// set controls to male
		if (ControllerUI::AvatarCreateWizard::GenderMale->isSelected())
		{
			hair->setMaxValue((float)::System::Math::Max(creationInfo->MaleHairIDs->Length - 1, 0));
			eyes->setMaxValue((float)::System::Math::Max(creationInfo->MaleEyeIDs->Length - 1, 0));
			nose->setMaxValue((float)::System::Math::Max(creationInfo->MaleNoseIDs->Length - 1, 0));
			mouth->setMaxValue((float)::System::Math::Max(creationInfo->MaleMouthIDs->Length - 1, 0));			
		}

		// or female
		else
		{
			hair->setMaxValue((float)::System::Math::Max(creationInfo->FemaleHairIDs->Length - 1, 0));
			eyes->setMaxValue((float)::System::Math::Max(creationInfo->FemaleEyeIDs->Length - 1, 0));
			nose->setMaxValue((float)::System::Math::Max(creationInfo->FemaleNoseIDs->Length - 1, 0));
			mouth->setMaxValue((float)::System::Math::Max(creationInfo->FemaleMouthIDs->Length - 1, 0));		
		}
		
		// finally reset the datamodel for image
		OgreClient::Singleton->Data->CharCreationInfo->SetExampleModel(
			ControllerUI::AvatarCreateWizard::GenderMale->isSelected() ? Gender::Male : Gender::Female,
			(int)ControllerUI::AvatarCreateWizard::SkinColor->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::HairColor->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Hair->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Eyes->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Nose->getCurrentValue(),
			(int)ControllerUI::AvatarCreateWizard::Mouth->getCurrentValue());
		
		return true;
	};
	
	bool UICallbacks::AvatarCreateWizard::OnProfileChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		
		if (!newval || newval == STRINGEMPTY)
			return true;

		CharCreationInfo^ data = OgreClient::Singleton->Data->CharCreationInfo;

		if (newval == UI_AVATARCREATEWIZARD_PROFILE_WARRIOR)
			data->SetAttributesToWarrior();
		
		else if (newval == UI_AVATARCREATEWIZARD_PROFILE_MAGE)
			data->SetAttributesToMage();

		else if (newval == UI_AVATARCREATEWIZARD_PROFILE_HYBRID)
			data->SetAttributesToHybrid();

		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnAttributeMouseMoveClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		
		if ((args.sysKeys & CEGUI::LeftMouse) || (args.button == CEGUI::MouseButton::LeftButton))
		{
			// calc
			float mouse_x = args.position.d_x;
			float control_x = args.window->getPixelPosition().d_x;
			float control_width = MathUtil::Max(args.window->getPixelSize().d_width, 0.1f);		
			float p = (mouse_x - control_x) / control_width;
			unsigned int val = ::System::Convert::ToUInt32(p * CharCreationInfo::ATTRIBUTE_MAXVALUE);

			if (args.window == ControllerUI::AvatarCreateWizard::Might)			
				OgreClient::Singleton->Data->CharCreationInfo->Might = val;

			else if (args.window == ControllerUI::AvatarCreateWizard::Intellect)			
				OgreClient::Singleton->Data->CharCreationInfo->Intellect = val;

			else if (args.window == ControllerUI::AvatarCreateWizard::Stamina)			
				OgreClient::Singleton->Data->CharCreationInfo->Stamina = val;

			else if (args.window == ControllerUI::AvatarCreateWizard::Agility)			
				OgreClient::Singleton->Data->CharCreationInfo->Agility = val;

			else if (args.window == ControllerUI::AvatarCreateWizard::Mysticism)			
				OgreClient::Singleton->Data->CharCreationInfo->Mysticism = val;

			else if (args.window == ControllerUI::AvatarCreateWizard::Aim)			
				OgreClient::Singleton->Data->CharCreationInfo->Aim = val;

			ControllerUI::AvatarCreateWizard::Profiles->setText(UI_AVATARCREATEWIZARD_PROFILE_CUSTOM);
			ControllerUI::AvatarCreateWizard::Profiles->selectListItemWithEditboxText();
		}

		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSpellDoubleClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// select spell
		if (args.button == CEGUI::MouseButton::LeftButton)		
			OgreClient::Singleton->Data->CharCreationInfo->SelectSpell(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSkillDoubleClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;
		
		// select skill
		if (args.button == CEGUI::MouseButton::LeftButton)		
			OgreClient::Singleton->Data->CharCreationInfo->SelectSkill(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSelectedSpellDoubleClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// deselect spell
		if (args.button == CEGUI::MouseButton::LeftButton)		
			OgreClient::Singleton->Data->CharCreationInfo->DeselectSpell(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSelectedSkillDoubleClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// deselect skill
		if (args.button == CEGUI::MouseButton::LeftButton)		
			OgreClient::Singleton->Data->CharCreationInfo->DeselectSkill(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSpellClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// select spell
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->Data->CharCreationInfo->SelectSpell(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSkillClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;
		
		// select skill
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->Data->CharCreationInfo->SelectSkill(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSelectedSpellClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// deselect spell
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->Data->CharCreationInfo->DeselectSpell(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnSelectedSkillClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// deselect skill
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->Data->CharCreationInfo->DeselectSkill(itm->getID());					
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnButtonBackClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		CEGUI::TabControl* tabCtr			= ControllerUI::AvatarCreateWizard::TabControl;

		// get current index
		size_t index = ControllerUI::AvatarCreateWizard::TabControl->getSelectedTabIndex();

		// back
		if (index > 0)		
			tabCtr->setSelectedTabAtIndex(index - 1);
		
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnButtonNextClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		CEGUI::TabControl* tabCtr			= ControllerUI::AvatarCreateWizard::TabControl;

		// get current index
		size_t index = ControllerUI::AvatarCreateWizard::TabControl->getSelectedTabIndex();

		// next
		if (index < tabCtr->getTabCount() - 1)		
			tabCtr->setSelectedTabAtIndex(index + 1);
		
		// finish
		else
		{
			// set name
			OgreClient::Singleton->Data->CharCreationInfo->AvatarName = 
				StringConvert::CEGUIToCLR(ControllerUI::AvatarCreateWizard::Name->getText());

			// set description
			OgreClient::Singleton->Data->CharCreationInfo->AvatarDescription = 
				StringConvert::CEGUIToCLR(ControllerUI::AvatarCreateWizard::Description->getText());

			// request creation
			OgreClient::Singleton->SendSystemMessageNewCharInfo();
		}
		return true;
	};

	bool UICallbacks::AvatarCreateWizard::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// switch back to avatar selection
		OgreClient::Singleton->Data->UIMode = UIMode::AvatarSelection;

		// mark GUIroot active
		//ControllerUI::ActivateRoot();

		return true;
	}

	bool UICallbacks::AvatarCreateWizard::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			OgreClient::Singleton->Data->UIMode = UIMode::AvatarSelection;
		}

		return true;
	}
};};
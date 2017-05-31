#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::GuildCreate::Initialize()
	{
		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_GUILDCREATE_WINDOW));
		GuildNameDesc	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_GUILDCREATE_GUILDNAMEDESC));
		GuildName		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_GUILDNAME));	
		MaleRanksDesc	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_GUILDCREATE_MALERANKSDESC));
		FemaleRanksDesc	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_GUILDCREATE_FEMALERANKSDESC));
		MaleRank1		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_MALERANK1));
		MaleRank2		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_MALERANK2));
		MaleRank3		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_MALERANK3));
		MaleRank4		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_MALERANK4));
		MaleRank5		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_MALERANK5));
		FemaleRank1		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_FEMALERANK1));
		FemaleRank2		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_FEMALERANK2));
		FemaleRank3		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_FEMALERANK3));
		FemaleRank4		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_FEMALERANK4));
		FemaleRank5		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_FEMALERANK5));
		SecretGuild		= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_GUILDCREATE_SECRETGUILD));
		CostDesc		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_GUILDCREATE_COSTDESC));
		Cost			= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDCREATE_COST));
		Create			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_GUILDCREATE_CREATE));
		
		// server server-side maximum string lengths
		GuildName->setMaxTextLength(BlakservStringLengths::MAX_GUILD_NAME_LEN);
		MaleRank1->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		MaleRank2->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		MaleRank3->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		MaleRank4->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		MaleRank5->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		FemaleRank1->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		FemaleRank2->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		FemaleRank3->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		FemaleRank4->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);
		FemaleRank5->setMaxTextLength(BlakservStringLengths::MAX_GUILD_RANK_LEN);

		// attach listener to guildask data
		OgreClient::Singleton->Data->GuildAskData->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnGuildAskDataPropertyChanged);
        
		// subscribe togglebutton select change
		SecretGuild->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::GuildCreate::OnSecretGuildSelectChange));

		// subscribe send button
		Create->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::GuildCreate::OnCreateClicked));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::GuildCreate::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::GuildCreate::OnWindowKeyUp));
	};

	void ControllerUI::GuildCreate::Destroy()
	{	
		// detach listener to guildask data
		OgreClient::Singleton->Data->GuildAskData->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnGuildAskDataPropertyChanged);
        
	};

	void ControllerUI::GuildCreate::ApplyLanguage()
	{
	};

	void ControllerUI::GuildCreate::OnGuildAskDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		Data::Models::GuildAskData^ obj = OgreClient::Singleton->Data->GuildAskData;
		
		// visible
		if (CLRString::Equals(e->PropertyName, Data::Models::GuildAskData::PROPNAME_ISVISIBLE))
		{
			// hide or show
			Window->setVisible(obj->IsVisible);

			// bring to front
			if (obj->IsVisible)
				Window->moveToFront();
		}

		// costnormal
		else if (CLRString::Equals(e->PropertyName, Data::Models::GuildAskData::PROPNAME_COSTNORMAL))
		{
			if (!SecretGuild->isSelected())
				Cost->setText(StringConvert::CLRToCEGUI(obj->CostNormal.ToString()));
		}

		// costsecret
		else if (CLRString::Equals(e->PropertyName, Data::Models::GuildAskData::PROPNAME_COSTSECRET))
		{
			if (SecretGuild->isSelected())
				Cost->setText(StringConvert::CLRToCEGUI(obj->CostSecret.ToString()));
		}
	};

	bool UICallbacks::GuildCreate::OnSecretGuildSelectChange(const CEGUI::EventArgs& e)
	{
		if (!ControllerUI::GuildCreate::SecretGuild->isSelected())
		{
			ControllerUI::GuildCreate::Cost->setText(
				StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->GuildAskData->CostNormal.ToString()));
		}
		else
		{
			ControllerUI::GuildCreate::Cost->setText(
				StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->GuildAskData->CostSecret.ToString()));
		}

		return true;
	};

	bool UICallbacks::GuildCreate::OnCreateClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;		

		// request to create guild with user set data
		OgreClient::Singleton->SendUserCommandGuildCreate(
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::GuildName->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::MaleRank1->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::MaleRank2->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::MaleRank3->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::MaleRank4->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::MaleRank5->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::FemaleRank1->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::FemaleRank2->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::FemaleRank3->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::FemaleRank4->getText()),
			StringConvert::CEGUIToCLR(ControllerUI::GuildCreate::FemaleRank5->getText()),
			ControllerUI::GuildCreate::SecretGuild->isSelected());

		// mark hidden
		OgreClient::Singleton->Data->GuildAskData->IsVisible = false;

		ControllerUI::ActivateRoot();

		return true;
	};

	bool UICallbacks::GuildCreate::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// clear (view will react)
			OgreClient::Singleton->Data->GuildAskData->IsVisible = false;

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}

	bool UICallbacks::GuildCreate::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// set not visible in datalayer (view will react)
		OgreClient::Singleton->Data->GuildAskData->IsVisible = false;
		
		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}
};};

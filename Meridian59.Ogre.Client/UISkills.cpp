#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::Skills::Initialize()
   {
      // setup references to children from xml nodes
      Window   = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_SKILLS_WINDOW));
      List     = static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_SKILLS_LIST));

      // set window layout from config
      Window->setPosition(OgreClient::Singleton->Config->UILayoutSkills->getPosition());
      Window->setSize(OgreClient::Singleton->Config->UILayoutSkills->getSize());

      // attach listener to avatar skills
      OgreClient::Singleton->Data->AvatarSkills->ListChanged += 
         gcnew ListChangedEventHandler(OnSkillsListChanged);

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Skills::OnKeyUp));
   };

   void ControllerUI::Skills::Destroy()
   {
      // detach listener from avatar skills
      OgreClient::Singleton->Data->AvatarSkills->ListChanged -= 
         gcnew ListChangedEventHandler(OnSkillsListChanged);
   };

   void ControllerUI::Skills::ApplyLanguage()
   {
      Window->setText(GetLangWindowTitle(LANGSTR_WINDOW_TITLE::SKILLS));
   };

   void ControllerUI::Skills::OnSkillsListChanged(Object^ sender, ListChangedEventArgs^ e)
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
            SkillChange(e->NewIndex);
            break;
      }
   };

   void ControllerUI::Skills::SkillAdd(int Index)
   {
      CEGUI::WindowManager& wndMgr = CEGUI::WindowManager::getSingleton();
      StatList^ obj = OgreClient::Singleton->Data->AvatarSkills[Index];

      SkillObject^ skillObj =
         OgreClient::Singleton->Data->SkillObjects->GetItemByID(obj->ObjectID);

      // create widget (item)
      CEGUI::ItemEntry* widget;

      // Use the draggable spell icon type for active skills, allowing them to be hotkeyed.
      if (skillObj != nullptr && skillObj->IsActiveSkill)
      {
         // create widget (item)
         widget = (CEGUI::ItemEntry*)wndMgr.createWindow(UI_WINDOWTYPE_AVATARSPELLITEM);
         // get children
         CEGUI::DragContainer* dragger =
            (CEGUI::DragContainer*)widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);

         // subscribe drag start and end to draggable icon
         dragger->subscribeEvent(CEGUI::DragContainer::EventDragStarted, CEGUI::Event::Subscriber(UICallbacks::Skills::OnDragStarted));
         dragger->subscribeEvent(CEGUI::DragContainer::EventDragEnded, CEGUI::Event::Subscriber(UICallbacks::Skills::OnDragEnded));

      }
      // Use non-draggable icon for non-active skills.
      else
      {
         // create widget (item)
         widget = (CEGUI::ItemEntry*)wndMgr.createWindow(UI_WINDOWTYPE_AVATARSKILLITEM);
      }

      // set ID
      widget->setID(obj->ObjectID);

      // subscribe click event
      widget->subscribeEvent(
         CEGUI::ItemEntry::EventMouseClick, 
         CEGUI::Event::Subscriber(UICallbacks::Skills::OnItemClicked));

      // insert in ui-list
      if ((int)List->getItemCount() > Index)
         List->insertItem(widget, List->getItemFromIndex(Index));

      // or add
      else
         List->addItem(widget);

      // fix a big with last item not visible
      // when insertItem was used
      List->notifyScreenAreaChanged(true);

      // update values
      SkillChange(Index);
   };

   void ControllerUI::Skills::SkillRemove(int Index)
   {
      // check
      if ((int)List->getItemCount() > Index)		
         List->removeItem(List->getItemFromIndex(Index));
   };

   void ControllerUI::Skills::SkillChange(int Index)
   {
      StatList^ obj = OgreClient::Singleton->Data->AvatarSkills[Index];
      SkillObject^ skillObj =
         OgreClient::Singleton->Data->SkillObjects->GetItemByID(obj->ObjectID);

      // check
      if ((int)List->getItemCount() > Index)
      {
         CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);

         CEGUI::Window* icon;
         if (skillObj != nullptr && skillObj->IsActiveSkill)
         {
            CEGUI::DragContainer* dragger =
               (CEGUI::DragContainer*)wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);
            icon = dragger->getChildAtIdx(0);
         }
         else
         {
            icon = wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);
         }

         CEGUI::Window* name     = wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_NAME);
         CEGUI::Window* percent  = wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_PERCENT);

         // set name
         name->setText(StringConvert::CLRToCEGUI(obj->ResourceName));
         percent->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SkillPoints) + '%');

         // build imagename
         const ::Ogre::String& oStrName =
            StringConvert::CLRToOgre(UI_NAMEPREFIX_STATICICON + obj->ResourceIconName->ToLower() + "/0");

         // define image (use same name)
         CEGUI::ImageManager* imgMan = CEGUI::ImageManager::getSingletonPtr();

         // create image no the fly
         if (!imgMan->isDefined(oStrName))
         {
            if (obj->Resource != nullptr && obj->Resource->Frames->Count > 0)
            {
               Ogre::TextureManager& texMan = Ogre::TextureManager::getSingleton();

               // possibly create texture
               Util::CreateTextureA8R8G8B8(obj->Resource->Frames[0], oStrName, UI_RESGROUP_IMAGESETS, MIP_DEFAULT);

               // reget TexPtr (no return from function works, ugh..)
               TexturePtr texPtr = texMan.getByName(oStrName);

               if (texPtr)
               {
                  // possibly create cegui wrap around it
                  Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);

                  // set image
                  icon->setProperty(UI_PROPNAME_IMAGE, oStrName);
               }
            }
         }

         // set existing image
         else
            icon->setProperty(UI_PROPNAME_IMAGE, oStrName);
      }
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::Skills::OnKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
      ::CEGUI::ItemListbox* list = ControllerUI::Skills::List;
      SkillList^ skills = OgreClient::Singleton->Data->AvatarSkills;

      // 1) Return/Enter
      if (args.scancode == CEGUI::Key::Return ||
         args.scancode == CEGUI::Key::NumpadEnter)
      {
         ::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

         // activate chat if no selection
         if (!itm)
         {
            return UICallbacks::OnKeyUp(args);
         }

         // otherwise try to perform the skill
         else
            OgreClient::Singleton->SendReqPerformMessage(itm->getID());
      }

      // 2) ESC
      if (args.scancode == CEGUI::Key::Escape)
      {
         ::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

         // if selection, unset
         if (itm)
            list->clearAllSelections();

         // if no selection, close window
         else
         {
            args.window->hide();

            // mark GUIroot active
            ControllerUI::ActivateRoot();
         }
      }

      // 3) ArrowDown
      else if (args.scancode == CEGUI::Key::ArrowDown)
      {
         ::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

         if (itm)
         {
            size_t idx = list->getItemIndex(itm);
            idx++;

            if (idx < list->getItemCount())
            {
               ::CEGUI::ItemEntry* nextitm = list->getItemFromIndex(idx);
               nextitm->setSelected(true);
               list->ensureItemIsVisibleVert(*nextitm);
            }
         }
      }

      // 4) ArrowUp
      else if (args.scancode == CEGUI::Key::ArrowUp)
      {
         ::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

         if (itm)
         {
            size_t idx = list->getItemIndex(itm);

            if (idx > 0)
            {
               idx--;

               if (idx < list->getItemCount())
               {
                  ::CEGUI::ItemEntry* nextitm = list->getItemFromIndex(idx);
                  nextitm->setSelected(true);
                  list->ensureItemIsVisibleVert(*nextitm);
               }
            }
         }
      }

      // 5) Jump to item with startletter
      else
      {
         if (!ControllerInput::OISKeyboard)
            return true;

         // convert keycode to char
         const ::std::string& cstr = ControllerInput::OISKeyboard->getAsString((::OIS::KeyCode)args.scancode);

         // get skills with prefix
         SkillList^ items = skills->GetItemsByPrefix(StringConvert::OgreToCLR(cstr), false);

         // must have 1 match
         if (items->Count == 0)
            return true;

         // look it up by id
         for (size_t i = 0; i < list->getItemCount(); i++)
         {
            ::CEGUI::ItemEntry* itm = list->getItemFromIndex(i);

            if (itm->getID() != items[0]->ObjectID)
               continue;

            // select & scroll
            itm->setSelected(true);
            list->ensureItemIsVisibleVert(*itm);

            break;
         }
      }

      return true;
   };

   bool UICallbacks::Skills::OnItemClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args   = (const CEGUI::MouseEventArgs&)e;
      const CEGUI::ItemEntry* itm         = (CEGUI::ItemEntry*)args.window;

      // single rightclick requests object details
      if (args.button == CEGUI::MouseButton::RightButton)
         OgreClient::Singleton->SendReqLookMessage(itm->getID());

      return true;
   };

   bool UICallbacks::Skills::OnItemDoubleClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = (const CEGUI::MouseEventArgs&)e;
      const CEGUI::ItemEntry* itm = (CEGUI::ItemEntry*)args.window;

      // double leftclick performs skill
      if (args.button == CEGUI::MouseButton::LeftButton)
         OgreClient::Singleton->SendReqPerformMessage(itm->getID());

      return true;

   };
   bool UICallbacks::Skills::OnDragStarted(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);

      CEGUI::DragContainer* drag = (CEGUI::DragContainer*)args.window;

      if (!drag->isDraggingEnabled())
         return true;

      ControllerUI::Skills::Window->setUsingAutoRenderingSurface(false);
      return true;
   };

   bool UICallbacks::Skills::OnDragEnded(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
      ControllerUI::Skills::Window->setUsingAutoRenderingSurface(true);
      return true;
   };
};};

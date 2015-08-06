/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

#pragma once

#pragma managed(push, off)
#include "OgreRenderTarget.h"
#include "CEGUI/CEGUI.h"
#include "CEGUI/RendererModules/Ogre/Renderer.h"
#pragma managed(pop)

#include "ImageComposerCEGUI.h"

namespace Meridian59 { namespace Ogre
{
	using namespace System::ComponentModel;
	using namespace Meridian59::Common::Enums;
	using namespace Meridian59::Data::Models;
	using namespace Meridian59::Data;

	// forward declaration
	class LoadingBarResourceGroupListener;

	/// <summary>
    /// Controls the overlay UI
    /// </summary>
	public ref class ControllerUI abstract sealed
	{
	protected:		
		static ::CEGUI::OgreRenderer* renderer;
		static ::CEGUI::System* system;
		static ::CEGUI::GUIContext* guiContext;
		static ::CEGUI::Window* guiRoot;
		static ::CEGUI::MouseCursor* mouseCursor;
		static ::CEGUI::Scheme* scheme;
		static ::CEGUI::Window* topControl;
		static ::CEGUI::Window* focusedControl;
		static ::CEGUI::Window* movingWindow;
		static ::CEGUI::Key::Scan keyDown;
		static ::CEGUI::Key::Scan keyChar;
		static bool processingInput;
		static bool fastKeyRepeat;
		static ::CEGUI::Window* draggedWindow;
		static ControllerUI(void);

	public:		
		static bool IsInitialized;

		static void Initialize(::Ogre::RenderTarget* Target);
		static void Destroy();
		static void Tick(double Tick, double Span);
		static void ToggleVisibility(::CEGUI::Window* Window);
		static bool IsRecursiveChildOf(::CEGUI::Window* Child, ::CEGUI::Window* Parent);
		static void ActivateRoot();
		static void PasteFromClipboard(::CEGUI::Window* EditBox);
		static void CopyToClipboard(::CEGUI::Window* EditBox, bool Cut);
		
		static void SaveLayoutToConfig();

		static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);

		static void InjectMousePosition(float x, float y);
		static void InjectMouseWheelChange(float z);
		static void InjectMouseButtonDown(::CEGUI::MouseButton Button);
		static void InjectMouseButtonUp(::CEGUI::MouseButton Button);
		static void InjectKeyDown(::CEGUI::Key::Scan Key);
		static void InjectKeyUp(::CEGUI::Key::Scan Key);
		static void InjectChar(::CEGUI::Key::Scan Key);
			
		static property ::CEGUI::OgreRenderer* Renderer 
		{ 
			public: ::CEGUI::OgreRenderer* get() { return renderer; }
			protected: void set(::CEGUI::OgreRenderer* value) { renderer = value; } 
		};

		static property ::CEGUI::System* System 
		{ 
			public: ::CEGUI::System* get() { return system; }
			protected: void set(::CEGUI::System* value) { system = value; } 
		};

		static property ::CEGUI::Window* GUIRoot 
		{ 
			public: ::CEGUI::Window* get() { return guiRoot; }
			protected: void set(::CEGUI::Window* value) { guiRoot = value; } 
		};

		/// <summary>
		/// The UI control on top of the z-order at the mouse coordinates.
		/// Updated with mouse moves, does not automatically also have focus!
		/// </summary>
		static property ::CEGUI::Window* TopControl 
		{ 
			public: ::CEGUI::Window* get() { return topControl; }
			protected: void set(::CEGUI::Window* value) { topControl = value; } 
		};

		static property ::CEGUI::Window* FocusedControl 
		{ 
			public: ::CEGUI::Window* get() { return focusedControl; }
			protected: void set(::CEGUI::Window* value) { focusedControl = value; } 
		};

		static property ::CEGUI::Window* MovingWindow 
		{ 
			public: ::CEGUI::Window* get() { return movingWindow; }
			public: void set(::CEGUI::Window* value) { movingWindow = value; } 
		};

		static property bool ProcessingInput 
		{ 
			public: bool get() { return processingInput; }
			protected: void set(bool value) { processingInput = value; } 
		};

		static property ::CEGUI::Window* DraggedWindow
		{
			public: ::CEGUI::Window* get() { return draggedWindow; }
			public: void set(::CEGUI::Window* value) { draggedWindow = value; }
		};

		/// <summary>
		/// Returns true if TopControl is either null or should be ignored.
		/// If true then make sure to handle the mouse input in the engine.
		/// </summary>
		static property bool IgnoreTopControlForMouseInput
		{ 
			public: bool get() 
			{ 
				
				return
					ControllerUI::TopControl == nullptr ||
					ControllerUI::TopControl == ControllerUI::GUIRoot ||
					ControllerUI::TopControl == ControllerUI::SplashNotifier::Window ||
					ControllerUI::TopControl == ControllerUI::DraggedWindow ||
					(ControllerUI::TopControl->getParent() && ControllerUI::TopControl->getParent() == ControllerUI::DraggedWindow) ||
					ControllerUI::PlayerOverlays::IsOverlayWindow(ControllerUI::TopControl);
			}
		};

		static property ::CEGUI::MouseCursor* MouseCursor 
		{ 
			public: ::CEGUI::MouseCursor* get() { return mouseCursor; }
			protected: void set(::CEGUI::MouseCursor* value) { mouseCursor = value; } 
		};

#pragma region UI windows
	public:
		/// <summary>
		/// LoadingBar window
		/// </summary>
		ref class LoadingBar abstract sealed
		{
		private:
			static LoadingBarResourceGroupListener* groupListener = nullptr;
			static float stepSizeGroup;
			static float stepSizeContent;
			static int skipCounter;

		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::ProgressBar* Group = nullptr;
			static ::CEGUI::ProgressBar* Content = nullptr;

			static void Initialize();
			static void Destroy();
			static void Start(unsigned short numGroupsInit);
			static void Finish();

			static void ResourceGroupScriptingStarted(const String* groupName, size_t scriptCount);
			static void ScriptParseStarted(const String* scriptName, bool &skipThisScript);
			static void ScriptParseEnded(const String* scriptName, bool skipped);
			static void ResourceGroupLoadStarted(const String* groupName, size_t resourceCount);
			static void ResourceGroupPrepareStarted(const String* groupName, size_t resourceCount);
			static void ResourceLoadStarted(ResourcePtr resource);
			static void WorldGeometryStageStarted(const String* description);
			static void WorldGeometryStageEnded();

			static void OnPreloadingGroupStarted(Object^ sender, ::System::EventArgs^ e);
			static void OnPreloadingGroupEnded(Object^ sender, ::System::EventArgs^ e);
			static void OnPreloadingFile(Object^ sender, ::Meridian59::Files::ResourceManager::StringEventArgs^ e);
		};

		/// <summary>
		/// Welcome window
		/// </summary>
		ref class Welcome abstract sealed
		{
		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::ItemListbox* Avatars = nullptr;
			static ::CEGUI::PushButton* Select = nullptr;
			static ::CEGUI::MultiLineEditbox* MOTD = nullptr;

			static void Initialize();
			static void Destroy();

			static void OnCharactersListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnWelcomeInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			
			static void CharacterAdd(int Index);
			static void CharacterRemove(int Index);
		};

		/// <summary>
		/// Status bar on top
		/// </summary>
		ref class StatusBar abstract sealed
		{
		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Window* FPSDescription = nullptr;
			static ::CEGUI::Window* FPSValue = nullptr;
			static ::CEGUI::Window* RTTDescription = nullptr;
			static ::CEGUI::Window* RTTValue = nullptr;
			static ::CEGUI::Window* PlayersDescription = nullptr;
			static ::CEGUI::PushButton* PlayersValue = nullptr;
			static ::CEGUI::Window* MoodDescription = nullptr;
			static ::CEGUI::PushButton* MoodHappy = nullptr;
			static ::CEGUI::PushButton* MoodNeutral = nullptr;
			static ::CEGUI::PushButton* MoodSad = nullptr;
			static ::CEGUI::PushButton* MoodAngry = nullptr;
			static ::CEGUI::Window* SafetyDescription = nullptr;
			static ::CEGUI::PushButton* SafetyValue = nullptr;
			static ::CEGUI::Window* MTimeDescription = nullptr;
			static ::CEGUI::Window* MTimeValue = nullptr;
			static ::CEGUI::Window* RoomDescription = nullptr;
			static ::CEGUI::Window* RoomValue = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnRoomInformationPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnOnlinePlayersListChanged(Object^ sender, ListChangedEventArgs^ e);
		};

		/// <summary>
		/// OnlinePlayers window
		/// </summary>
		ref class OnlinePlayers abstract sealed
		{		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnOnlinePlayersListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnlinePlayerAdd(int Index);
			static void OnlinePlayerRemove(int Index);
			static void OnlinePlayerChange(int Index);
		};

		/// <summary>
		/// RoomObjects window
		/// </summary>
		ref class RoomObjects abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;
			static ::CEGUI::ToggleButton* ShowAll = nullptr;
			static ::CEGUI::ToggleButton* ShowGuild = nullptr;
			static ::CEGUI::ToggleButton* ShowEnemy = nullptr;
			static ::CEGUI::ToggleButton* ShowFriend = nullptr;
			static ::CEGUI::ToggleButton* ShowAttack = nullptr;
			static ::CEGUI::ToggleButton* ShowGet = nullptr;
			static ::CEGUI::ToggleButton* ShowMinion = nullptr;
			static ::CEGUI::ToggleButton* ShowPK = nullptr;
			static ::CEGUI::ToggleButton* ShowBuy = nullptr;

			static bool FlippedbyAll = false;

			static void Initialize();
			static void Destroy();
			static void OnRoomObjectsFilteredListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void RoomObjectAdd(int Index);
			static void RoomObjectRemove(int Index);
			static void RoomObjectChange(int Index);
		};

		/// <summary>
		/// Chat window
		/// </summary>
		ref class Chat abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* Text = nullptr;
			static ::CEGUI::Editbox* Input = nullptr;
			static ::CEGUI::Scrollbar* Scrollbar = nullptr;
			static ::System::Collections::Generic::Queue<::Meridian59::Data::Models::ServerString^>^ Queue = nullptr;
			static unsigned int DeleteCounter = 0;
			
			static void Initialize();
			static void Destroy();
			static void Tick(double Tick, double Span);
			static ::CEGUI::String GetChatString(ServerString^ ChatMessage);	
			static void OnChatMessagesListChanged(Object^ sender, ListChangedEventArgs^ e);	
		};

		/// <summary>
		/// Avatar window
		/// </summary>
		ref class Avatar abstract sealed
		{
		protected:
			static ImageComposerCEGUI<RoomObject^>^ imageComposerHead;
			static array<ImageComposerCEGUI<ObjectBase^>^>^ imageComposersBuffs;

		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Window* Head = nullptr;
			static ::CEGUI::GridLayoutContainer* Enchantments = nullptr;
			static ::CEGUI::VerticalLayoutContainer* Conditions = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnNewHeadImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnNewBuffImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnBuffListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnConditionListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void BuffAdd(int Index);
			static void BuffRemove(int Index);
			static void ConditionAdd(int Index);
			static void ConditionRemove(int Index);			
			static void ConditionChange(int Index);
		};

		/// <summary>
		/// ObjectDetails window
		/// </summary>
		ref class ObjectDetails abstract sealed
		{
		protected:
			static ImageComposerCEGUI<ObjectBase^>^ imageComposer;
		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* Image = nullptr;
			static ::CEGUI::Window* Name = nullptr;
			static ::CEGUI::MultiLineEditbox* Description = nullptr;
			static ::CEGUI::MultiLineEditbox* Inscription = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnLookObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void SetLayout(LookTypeFlags^ LayoutType);
		};

		/// <summary>
		/// PlayerDetails window
		/// </summary>
		ref class PlayerDetails abstract sealed
		{
		protected:
			static ImageComposerCEGUI<ObjectBase^>^ imageComposer;
		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* Image = nullptr;
			static ::CEGUI::Window* Name = nullptr;
			static ::CEGUI::Window* Titles = nullptr;
			static ::CEGUI::MultiLineEditbox* Description = nullptr;
			static ::CEGUI::Window* HomepageDescription= nullptr;
			static ::CEGUI::Editbox* HomepageValue = nullptr;
			static ::CEGUI::PushButton* OK = nullptr;
			

			static void Initialize();
			static void Destroy();
			static void OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnLookPlayerPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
		};

		/// <summary>
		/// Target window
		/// </summary>
		ref class Target abstract sealed
		{
		protected:
			static ImageComposerCEGUI<ObjectBase^>^ imageComposer;
			static ObjectBase^ targetObject;

		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Window* Image = nullptr;
			static ::CEGUI::Window* Name = nullptr;
			static ::CEGUI::HorizontalLayoutContainer* Layout = nullptr;
			static ::CEGUI::PushButton* Inspect = nullptr;
			static ::CEGUI::PushButton* Attack = nullptr;
			static ::CEGUI::PushButton* Activate = nullptr;
			static ::CEGUI::PushButton* Buy = nullptr;
			static ::CEGUI::PushButton* Trade = nullptr;
			static ::CEGUI::PushButton* Loot = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnTargetObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e);			
		};

		
		/// <summary>
		/// SplashNotifier window
		/// </summary>
		ref class SplashNotifier abstract sealed
		{
		protected:
			static ::System::Collections::Generic::List<::System::String^>^ notifications;

		public:
			static ::CEGUI::Window* Window = nullptr;

			static void Initialize();
			static void Destroy();
			static void UpdateNotification();
			static void ShowNotification(::System::String^ Text);
			static void HideNotification(::System::String^ Text);
			static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnParalyzePropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
		};

		/// <summary>
		/// MiniMap window
		/// </summary>
		ref class MiniMap abstract sealed
		{
		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Window* DrawSurface = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnImageChanged(Object^ sender, ::System::EventArgs^ e);
		};

		/// <summary>
		/// RoomEnchantments window
		/// </summary>
		ref class RoomEnchantments abstract sealed
		{
		protected:
			static array<ImageComposerCEGUI<ObjectBase^>^>^ imageComposersBuffs;

		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::GridLayoutContainer* Grid = nullptr;
			
			static void Initialize();
			static void Destroy();
			static void OnNewBuffImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnBuffListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void BuffAdd(int Index);
			static void BuffRemove(int Index);
		};

		/// <summary>
		/// Buy window
		/// </summary>
		ref class Buy abstract sealed
		{
		protected:
			static ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>^ imageComposers;
		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;
			static ::CEGUI::PushButton* OK = nullptr;
			static ::CEGUI::Window* SumDescription = nullptr;
			static ::CEGUI::Window* SumValue = nullptr;

			static void Initialize();
			static void Destroy();
			static void CalculateSum();
			static void OnBuyPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnBuyListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnNewBuyItemImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void BuyItemAdd(int Index);
			static void BuyItemRemove(int Index);
		};

		/// <summary>
		/// Attributes window
		/// </summary>
		ref class Attributes abstract sealed
		{		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnAttributesListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void AttributeAdd(int Index);
			static void AttributeRemove(int Index);
			static void AttributeChange(int Index);
		};

		/// <summary>
		/// Skills window
		/// </summary>
		ref class Skills abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnSkillsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void SkillAdd(int Index);
			static void SkillRemove(int Index);
			static void SkillChange(int Index);
		};

		/// <summary>
		/// Spells window
		/// </summary>
		ref class Spells abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnSpellsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void SpellAdd(int Index);
			static void SpellRemove(int Index);
			static void SpellChange(int Index);
		};

		/// <summary>
		/// Quests window
		/// </summary>
		ref class Quests abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnQuestsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void QuestAdd(int Index);
			static void QuestRemove(int Index);
			static void QuestChange(int Index);
		};

		/// <summary>
		/// Actions window
		/// </summary>
		ref class Actions abstract sealed
		{
		protected:
			static void CreateItem(AvatarAction Type);

		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;

			static void Initialize();
			static void Destroy();
		};

		/// <summary>
		/// Inventory window
		/// </summary>
		ref class Inventory abstract sealed
		{
		protected:
			static array<ImageComposerCEGUI<InventoryObject^>^>^ imageComposers;

		public:			
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ScrollablePane* Pane = nullptr;
			static ::CEGUI::GridLayoutContainer* List = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnInventoryListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void InventoryAdd(int Index);
			static void InventoryRemove(int Index);
			static void InventoryChange(int Index);
			static void Tick(double Tick, double Span);
			static void SwapImageComposers(unsigned int Index1, unsigned int Index2);

			static bool DoClick;
			static InventoryObject^ ClickObject;

		};

		/// <summary>
		/// MainButtonsLeft window
		/// </summary>
		ref class MainButtonsLeft abstract sealed
		{
		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Window* Chat = nullptr;
			static ::CEGUI::Window* Guild = nullptr;
			static ::CEGUI::Window* Mail = nullptr;
			static ::CEGUI::Window* Map = nullptr;
			static ::CEGUI::Window* Options = nullptr;

			static void Initialize();
			static void Destroy();
		};

		/// <summary>
		/// MainButtonsRight window
		/// </summary>
		ref class MainButtonsRight abstract sealed
		{
		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Window* Inventory = nullptr;
			static ::CEGUI::Window* Spells = nullptr;
			static ::CEGUI::Window* Skills = nullptr;
			static ::CEGUI::Window* Actions = nullptr;
			static ::CEGUI::Window* Attributes = nullptr;
			static ::CEGUI::Window* Quests = nullptr;

			static void Initialize();
			static void Destroy();
		};

		/// <summary>
		/// Amount window
		/// </summary>
		ref class Amount abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Editbox* Value = nullptr;
			static ::CEGUI::PushButton* OK = nullptr;
			static unsigned int ID = 0;
			
			static void Initialize();
			static void Destroy();
			static void Drop();
			static void ShowValues(unsigned int ID, unsigned int Count);
		};

		/// <summary>
		/// Trade window
		/// </summary>
		ref class Trade abstract sealed
		{
		protected:
			static ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>^ imageComposersYou;
			static ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>^ imageComposersPartner;

		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* NameYou = nullptr;
			static ::CEGUI::Window* NamePartner = nullptr;
			static ::CEGUI::ItemListbox* ListYou = nullptr;
			static ::CEGUI::ItemListbox* ListPartner = nullptr;
			static ::CEGUI::PushButton* Offer = nullptr;
			static ::CEGUI::PushButton* Accept = nullptr;
			
			static void Initialize();
			static void Destroy();
			static void OnTradePropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnItemsYouListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnItemsPartnerListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnNewItemYouImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnNewItemPartnerImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void ItemYouAdd(int Index);
			static void ItemYouRemove(int Index);
			static void ItemPartnerAdd(int Index);
			static void ItemPartnerRemove(int Index);			
		};

		/// <summary>
		/// ActionButtons window
		/// </summary>
		ref class ActionButtons abstract sealed
		{
		protected:
			static array<ImageComposerCEGUI<ObjectBase^>^>^ imageComposers;

		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::GridLayoutContainer* Grid = nullptr;
			
			static void Initialize();
			static void Destroy();
			static void OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnActionButtonsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void ActionButtonAdd(int Index);
			static void ActionButtonRemove(int Index);
			static void ActionButtonChange(int Index);
		};

		/// <summary>
		/// NewsGroup window
		/// </summary>
		ref class NewsGroup abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::MultiColumnList* List = nullptr;
			static ::CEGUI::Window* HeadLine = nullptr;
			static ::CEGUI::PushButton* Create = nullptr;
			static ::CEGUI::PushButton* Respond = nullptr;
			static ::CEGUI::PushButton* MailAuthor = nullptr;
			static ::CEGUI::PushButton* Refresh = nullptr;
			static ::CEGUI::MultiLineEditbox* Text = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnNewsGroupPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnArticleHeadListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void ArticleHeadAdd(int Index);
			static void ArticleHeadRemove(int Index);
			static void ArticleHeadChange(int Index);			
		};

		/// <summary>
		/// NewsGroupCompose window
		/// </summary>
		ref class NewsGroupCompose abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* GroupDesc = nullptr;
			static ::CEGUI::Window* Group = nullptr;
			static ::CEGUI::Window* HeadLineDesc = nullptr;
			static ::CEGUI::Editbox* HeadLine = nullptr;
			static ::CEGUI::MultiLineEditbox* Text = nullptr;
			static ::CEGUI::PushButton* Send = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnNewsGroupPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
		};

		/// <summary>
		/// Mail window
		/// </summary>
		ref class Mail abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::MultiColumnList* List = nullptr;
			static ::CEGUI::PushButton* Create = nullptr;
			static ::CEGUI::PushButton* Respond = nullptr;
			static ::CEGUI::PushButton* RespondAll = nullptr;
			static ::CEGUI::PushButton* Refresh = nullptr;
			static ::CEGUI::MultiLineEditbox* Text = nullptr;

			static void Initialize();
			static void Destroy();
			static void OnMailsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void MailAdd(int Index);
			static void MailRemove(int Index);
			static void MailChange(int Index);			
		};

		/// <summary>
		/// MailCompose window
		/// </summary>
		ref class MailCompose abstract sealed
		{
		public:
			static array<::System::String^>^ LastLookupNames = nullptr;
			
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* Error = nullptr;
			static ::CEGUI::Window* RecipientsDesc = nullptr;
			static ::CEGUI::Editbox* Recipients = nullptr;
			static ::CEGUI::Window* HeadLineDesc = nullptr;
			static ::CEGUI::Editbox* HeadLine = nullptr;
			static ::CEGUI::MultiLineEditbox* Text = nullptr;
			static ::CEGUI::PushButton* Send = nullptr;

			static void Initialize();
			static void Destroy();
			static void ProcessResult(array<ObjectID^>^ Result);
		};

		/// <summary>
		/// Guild window
		/// </summary>
		ref class Guild abstract sealed
		{
		protected:
			static ImageComposerCEGUI<ObjectBase^>^ imageComposerShield;

		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::TabControl* TabControl = nullptr;
			static ::CEGUI::Window* TabMembers = nullptr;
			static ::CEGUI::Window* TabDiplomacy = nullptr;
			static ::CEGUI::Window* TabGuildmaster = nullptr;
			static ::CEGUI::Window* TabShield = nullptr;
			static ::CEGUI::PushButton* Renounce = nullptr;
			static ::CEGUI::ItemListbox* ListMembers = nullptr;
			static ::CEGUI::ItemListbox* ListGuilds = nullptr;
			static ::CEGUI::Window* PasswordDesc = nullptr;
			static ::CEGUI::Editbox* PasswordVal = nullptr;
			static ::CEGUI::PushButton* SetPassword = nullptr;
			static ::CEGUI::Window* ShieldImage = nullptr;
			static ::CEGUI::Window* ShieldColor1Desc = nullptr;
			static ::CEGUI::Slider* ShieldColor1 = nullptr;
			static ::CEGUI::Window* ShieldColor2Desc = nullptr;
			static ::CEGUI::Slider* ShieldColor2 = nullptr;
			static ::CEGUI::Window* ShieldDesignDesc = nullptr;
			static ::CEGUI::Slider* ShieldDesign = nullptr;
			static ::CEGUI::Window* ShieldClaimedByDesc = nullptr;
			static ::CEGUI::Window* ShieldClaimedBy = nullptr;
			static ::CEGUI::PushButton* ShieldClaim = nullptr;
			
			static void Initialize();
			static void Destroy();
			static void OnGuildInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnGuildShieldInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnMembersListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnGuildsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnNewShieldImageAvailable(Object^ sender, ::System::EventArgs^ e);			
			static void MemberAdd(int Index);
			static void MemberRemove(int Index);
			static void MemberChange(int Index);
			static void GuildAdd(int Index);
			static void GuildRemove(int Index);
			static void GuildChange(int Index);
		};

		/// <summary>
		/// GuildCreate window
		/// </summary>
		ref class GuildCreate abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* GuildNameDesc = nullptr;
			static ::CEGUI::Editbox* GuildName = nullptr;
			static ::CEGUI::Window* MaleRanksDesc = nullptr;
			static ::CEGUI::Window* FemaleRanksDesc = nullptr;
			static ::CEGUI::Editbox* MaleRank1 = nullptr;
			static ::CEGUI::Editbox* MaleRank2 = nullptr;
			static ::CEGUI::Editbox* MaleRank3 = nullptr;
			static ::CEGUI::Editbox* MaleRank4 = nullptr;
			static ::CEGUI::Editbox* MaleRank5 = nullptr;
			static ::CEGUI::Editbox* FemaleRank1 = nullptr;
			static ::CEGUI::Editbox* FemaleRank2 = nullptr;
			static ::CEGUI::Editbox* FemaleRank3 = nullptr;
			static ::CEGUI::Editbox* FemaleRank4 = nullptr;
			static ::CEGUI::Editbox* FemaleRank5 = nullptr;
			static ::CEGUI::ToggleButton* SecretGuild = nullptr;
			static ::CEGUI::Window* CostDesc = nullptr;
			static ::CEGUI::Window* Cost = nullptr;
			static ::CEGUI::PushButton* Create = nullptr;
			
			static void Initialize();
			static void Destroy();
			static void OnGuildAskDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
		};

		/// <summary>
		/// AvatarCreateWizard window
		/// </summary>
		ref class AvatarCreateWizard abstract sealed
		{
		protected:
			static ImageComposerCEGUI<ObjectBase^>^ imageComposerHead;
		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::TabControl* TabControl = nullptr;
			static ::CEGUI::Window* TabBasic = nullptr;
			static ::CEGUI::Window* TabAttributes = nullptr;
			static ::CEGUI::Window* TabSpellsSkills = nullptr;

			static ::CEGUI::Window* NameDesc = nullptr;
			static ::CEGUI::Editbox* Name = nullptr;
			static ::CEGUI::Window* Image = nullptr;
			static ::CEGUI::Window* GenderDesc = nullptr;
			static ::CEGUI::RadioButton* GenderMale = nullptr;
			static ::CEGUI::RadioButton* GenderFemale = nullptr;
			static ::CEGUI::Window* SkinColorDesc = nullptr;
			static ::CEGUI::Slider* SkinColor = nullptr;
			static ::CEGUI::Window* HairDesc = nullptr;
			static ::CEGUI::Slider* Hair = nullptr;
			static ::CEGUI::Window* HairColorDesc = nullptr;
			static ::CEGUI::Slider* HairColor = nullptr;
			static ::CEGUI::Window* EyesDesc = nullptr;
			static ::CEGUI::Slider* Eyes = nullptr;
			static ::CEGUI::Window* NoseDesc = nullptr;
			static ::CEGUI::Slider* Nose = nullptr;
			static ::CEGUI::Window* MouthDesc = nullptr;
			static ::CEGUI::Slider* Mouth = nullptr;
			static ::CEGUI::Window* DescriptionDesc = nullptr;
			static ::CEGUI::MultiLineEditbox* Description = nullptr;
			
			static ::CEGUI::Window* ProfilesDesc = nullptr;
			static ::CEGUI::Combobox* Profiles = nullptr;
			static ::CEGUI::Window* MightDesc = nullptr;
			static ::CEGUI::ProgressBar* Might = nullptr;
			static ::CEGUI::Window* IntellectDesc = nullptr;
			static ::CEGUI::ProgressBar* Intellect = nullptr;
			static ::CEGUI::Window* StaminaDesc = nullptr;
			static ::CEGUI::ProgressBar* Stamina = nullptr;
			static ::CEGUI::Window* AgilityDesc = nullptr;
			static ::CEGUI::ProgressBar* Agility = nullptr;
			static ::CEGUI::Window* MysticismDesc = nullptr;
			static ::CEGUI::ProgressBar* Mysticism = nullptr;
			static ::CEGUI::Window* AimDesc = nullptr;
			static ::CEGUI::ProgressBar* Aim = nullptr;
			static ::CEGUI::Window* AttributesAvailableDesc = nullptr;
			static ::CEGUI::ProgressBar* AttributesAvailable = nullptr;

			static ::CEGUI::Window* SpellsDesc = nullptr;
			static ::CEGUI::ItemListbox* Spells = nullptr;
			static ::CEGUI::Window* SkillsDesc = nullptr;
			static ::CEGUI::ItemListbox* Skills = nullptr;
			static ::CEGUI::Window* SelectedSpellsDesc = nullptr;
			static ::CEGUI::ItemListbox* SelectedSpells = nullptr;
			static ::CEGUI::Window* SelectedSkillsDesc = nullptr;
			static ::CEGUI::ItemListbox* SelectedSkills = nullptr;
			static ::CEGUI::Window* SkillPointsAvailableDesc = nullptr;
			static ::CEGUI::ProgressBar* SkillPointsAvailable = nullptr;

			static ::CEGUI::PushButton* ButtonBack = nullptr;
			static ::CEGUI::PushButton* ButtonNext = nullptr;
			static ::CEGUI::Window* DataOK = nullptr;
			
			static void Initialize();
			static void Destroy();

			static void OnCharCreationInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnNewHeadImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void OnSpellsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnSkillsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnSelectedSpellsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnSelectedSkillsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void SpellAdd(int Index);
			static void SpellRemove(int Index);
			static void SkillAdd(int Index);
			static void SkillRemove(int Index);
			static void SelectedSpellAdd(int Index);
			static void SelectedSpellRemove(int Index);
			static void SelectedSkillAdd(int Index);
			static void SelectedSkillRemove(int Index);
		};
		
		/// <summary>
		/// ConfirmPopup window
		/// </summary>
		ref class ConfirmPopup abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::Window* Text = nullptr;
			static ::CEGUI::PushButton* Yes = nullptr;
			static ::CEGUI::PushButton* No = nullptr;

			static void Initialize();
			static void Destroy();			
		};

		/// <summary>
		/// PlayerOverlays window
		/// </summary>
		ref class PlayerOverlays abstract sealed
		{
		protected:
			static ::System::Collections::Generic::List<ImageComposerCEGUI<PlayerOverlay^>^>^ imageComposers;
			static ::std::vector<::CEGUI::Window*>* overlayWindows;
			static ::CEGUI::UVector2 GetScreenPositionForHotspot(ImageComposerCEGUI<PlayerOverlay^>^ ImageComposer);
			static float scale;

		public:
			static void Initialize();
			static void Destroy();
			static bool IsOverlayWindow(::CEGUI::Window* Window);
			static void OnPlayerOverlaysListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void PlayerOverlayAdd(int Index);
			static void PlayerOverlayRemove(int Index);
			static void PlayerOverlayChange(int Index);
			static void OnImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void HideOverlays();
			static void ShowOverlays();
			static void WindowResized(int Width, int Height);
		};

		/// <summary>
		/// ObjectContents window
		/// </summary>
		ref class ObjectContents abstract sealed
		{		
		protected:
			static ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>^ imageComposers;
		
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			static ::CEGUI::ItemListbox* List = nullptr;
			static ::CEGUI::PushButton* Get = nullptr;
			
			static void Initialize();
			static void Destroy();
			static void OnObjectContentsPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnObjectContentsListChanged(Object^ sender, ListChangedEventArgs^ e);
			static void OnNewItemImageAvailable(Object^ sender, ::System::EventArgs^ e);
			static void ItemAdd(int Index);
			static void ItemRemove(int Index);
			static void ItemChange(int Index);
		};
		
		/// <summary>
		/// Login window
		/// </summary>
		ref class Login abstract sealed
		{
		public:
			static ::CEGUI::Window* Window = nullptr;
			static ::CEGUI::Combobox* Server = nullptr;
			static ::CEGUI::Editbox* Username = nullptr;
			static ::CEGUI::Editbox* Password = nullptr;

			static ::CEGUI::PushButton* Connect = nullptr;
			static ::CEGUI::PushButton* Options = nullptr;

			static void Initialize();
			static void Destroy();
		};

		/// <summary>
		/// Options window
		/// </summary>
		ref class Options abstract sealed
		{
		public:
			static ::CEGUI::FrameWindow* Window = nullptr;
			
			static ::CEGUI::PushButton* Engine = nullptr;
			static ::CEGUI::PushButton* Input = nullptr;
			static ::CEGUI::PushButton* UI = nullptr;
			static ::CEGUI::PushButton* Aliases = nullptr;
			static ::CEGUI::PushButton* About = nullptr;

			static ::CEGUI::TabControl* TabControl = nullptr;
			static ::CEGUI::Window* TabEngine = nullptr;
			static ::CEGUI::Window* TabInput = nullptr;
			static ::CEGUI::Window* TabUI = nullptr;
			static ::CEGUI::Window* TabAliases = nullptr;
			static ::CEGUI::Window* TabAbout = nullptr;

			static ::CEGUI::TabControl* TabInputTabControl = nullptr;
			static ::CEGUI::Window* TabInputTabGeneral = nullptr;
			static ::CEGUI::Window* TabInputTabActionButtons1 = nullptr;
			static ::CEGUI::Window* TabInputTabActionButtons2 = nullptr;

			//
			static ::CEGUI::Combobox* Display = nullptr;
			static ::CEGUI::Combobox* Resolution = nullptr;
			static ::CEGUI::ToggleButton* WindowMode = nullptr;
			static ::CEGUI::ToggleButton* WindowBorders = nullptr;
			static ::CEGUI::ToggleButton* VSync = nullptr;
			static ::CEGUI::Combobox* FSAA = nullptr;
			static ::CEGUI::Combobox* Filtering = nullptr;
			static ::CEGUI::Combobox* ImageBuilder = nullptr;
			static ::CEGUI::Combobox* ScalingQuality = nullptr;
			static ::CEGUI::Combobox* TextureQuality = nullptr;

			static ::CEGUI::ToggleButton* DisableMipmaps = nullptr;
			static ::CEGUI::ToggleButton* DisableNewRoomTextures = nullptr;
			static ::CEGUI::ToggleButton* Disable3DModels = nullptr;
			static ::CEGUI::ToggleButton* DisableNewSky = nullptr;
			static ::CEGUI::ToggleButton* DisableWeather = nullptr;

			static ::CEGUI::Slider* Particles = nullptr;
			static ::CEGUI::Slider* Decoration = nullptr;
			static ::CEGUI::Slider* MusicVolume = nullptr;
			static ::CEGUI::Slider* SoundVolume = nullptr;
			static ::CEGUI::ToggleButton* DisableLoopSounds = nullptr;

			static ::CEGUI::ToggleButton* PreloadRooms = nullptr;
			static ::CEGUI::ToggleButton* PreloadRoomTextures = nullptr;
			static ::CEGUI::ToggleButton* PreloadObjects = nullptr;
			static ::CEGUI::ToggleButton* PreloadSounds = nullptr;
			static ::CEGUI::ToggleButton* PreloadMusic = nullptr;

			//

			static ::CEGUI::PushButton* LearnMoveForward = nullptr;
			static ::CEGUI::PushButton* LearnMoveBackward = nullptr;
			static ::CEGUI::PushButton* LearnMoveLeft = nullptr;
			static ::CEGUI::PushButton* LearnMoveRight = nullptr;
			static ::CEGUI::PushButton* LearnRotateLeft = nullptr;
			static ::CEGUI::PushButton* LearnRotateRight = nullptr;
			static ::CEGUI::PushButton* LearnWalk = nullptr;
			static ::CEGUI::PushButton* LearnAutoMove = nullptr;
			static ::CEGUI::PushButton* LearnNextTarget = nullptr;
			static ::CEGUI::PushButton* LearnSelfTarget = nullptr;
			static ::CEGUI::PushButton* LearnOpen = nullptr;
			static ::CEGUI::PushButton* LearnClose = nullptr;
			static ::CEGUI::Slider* MouseAimSpeed = nullptr;
			static ::CEGUI::Slider* KeyRotateSpeed = nullptr;
			static ::CEGUI::Combobox* RightClickAction = nullptr;
			static ::CEGUI::ToggleButton* InvertMouseY = nullptr;
			static ::CEGUI::PushButton* LearnAction01 = nullptr;
			static ::CEGUI::PushButton* LearnAction02 = nullptr;
			static ::CEGUI::PushButton* LearnAction03 = nullptr;
			static ::CEGUI::PushButton* LearnAction04 = nullptr;
			static ::CEGUI::PushButton* LearnAction05 = nullptr;
			static ::CEGUI::PushButton* LearnAction06 = nullptr;
			static ::CEGUI::PushButton* LearnAction07 = nullptr;
			static ::CEGUI::PushButton* LearnAction08 = nullptr;
			static ::CEGUI::PushButton* LearnAction09 = nullptr;
			static ::CEGUI::PushButton* LearnAction10 = nullptr;
			static ::CEGUI::PushButton* LearnAction11 = nullptr;
			static ::CEGUI::PushButton* LearnAction12 = nullptr;
			static ::CEGUI::PushButton* LearnAction13 = nullptr;
			static ::CEGUI::PushButton* LearnAction14 = nullptr;
			static ::CEGUI::PushButton* LearnAction15 = nullptr;
			static ::CEGUI::PushButton* LearnAction16 = nullptr;
			static ::CEGUI::PushButton* LearnAction17 = nullptr;
			static ::CEGUI::PushButton* LearnAction18 = nullptr;
			static ::CEGUI::PushButton* LearnAction19 = nullptr;
			static ::CEGUI::PushButton* LearnAction20 = nullptr;
			static ::CEGUI::PushButton* LearnAction21 = nullptr;
			static ::CEGUI::PushButton* LearnAction22 = nullptr;
			static ::CEGUI::PushButton* LearnAction23 = nullptr;
			static ::CEGUI::PushButton* LearnAction24 = nullptr;
			static ::CEGUI::PushButton* LearnAction25 = nullptr;
			static ::CEGUI::PushButton* LearnAction26 = nullptr;
			static ::CEGUI::PushButton* LearnAction27 = nullptr;
			static ::CEGUI::PushButton* LearnAction28 = nullptr;
			static ::CEGUI::PushButton* LearnAction29 = nullptr;
			static ::CEGUI::PushButton* LearnAction30 = nullptr;
			static ::CEGUI::PushButton* LearnAction31 = nullptr;
			static ::CEGUI::PushButton* LearnAction32 = nullptr;
			static ::CEGUI::PushButton* LearnAction33 = nullptr;
			static ::CEGUI::PushButton* LearnAction34 = nullptr;
			static ::CEGUI::PushButton* LearnAction35 = nullptr;
			static ::CEGUI::PushButton* LearnAction36 = nullptr;
			static ::CEGUI::PushButton* LearnAction37 = nullptr;
			static ::CEGUI::PushButton* LearnAction38 = nullptr;
			static ::CEGUI::PushButton* LearnAction39 = nullptr;
			static ::CEGUI::PushButton* LearnAction40 = nullptr;
			static ::CEGUI::PushButton* LearnAction41 = nullptr;
			static ::CEGUI::PushButton* LearnAction42 = nullptr;
			static ::CEGUI::PushButton* LearnAction43 = nullptr;
			static ::CEGUI::PushButton* LearnAction44 = nullptr;
			static ::CEGUI::PushButton* LearnAction45 = nullptr;
			static ::CEGUI::PushButton* LearnAction46 = nullptr;
			static ::CEGUI::PushButton* LearnAction47 = nullptr;
			static ::CEGUI::PushButton* LearnAction48 = nullptr;

			//
			static ::CEGUI::ItemListbox* ListAliases = nullptr;
			static ::CEGUI::Editbox* AliasKey = nullptr;
			static ::CEGUI::Editbox* AliasValue = nullptr;
			static ::CEGUI::PushButton* AliasAddBtn = nullptr;

			//
			static ::CEGUI::TabControl* TabAboutTabControl = nullptr;
			static ::CEGUI::Window* TabAboutTabGeneral = nullptr;
			static ::CEGUI::Window* TabAboutTabHistory = nullptr;
			
			static ::CEGUI::TabControl* TabAboutTabHistoryTabControl = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabEvolution = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabResurrection = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabDarkAuspices = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabInsurrection = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabRenaissance = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabRevelation = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabValeOfSorrow = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryTabTheInternetQuestBegins = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageEvolution = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageResurrection = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageDarkAuspices = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageInsurrection = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageRenaissance = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageRevelation = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageValeOfSorrow = nullptr;
			static ::CEGUI::Window* TabAboutTabHistoryImageTheInternetQuestBegins = nullptr;

			static void AliasAdd(int Index);
			static void AliasRemove(int Index);

			static void OnConfigPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
			static void OnAliasListChanged(Object^ sender, ListChangedEventArgs^ e);

			static void Initialize();
			static void Destroy();
		};
#pragma endregion
	};

#pragma region UICallbacks
	/// <summary>
	/// Native event handlers for CEGUI event subscriptions
	/// </summary>
	public class UICallbacks
	{
	public:
		static bool OnWindowClosed(const CEGUI::EventArgs& e);
		static bool OnKeyUp(const CEGUI::EventArgs& e);
		static bool OnRootMouseDown(const CEGUI::EventArgs& e);
		static bool OnRootKeyDown(const CEGUI::EventArgs& e);
		static bool OnCopyPasteKeyDown(const CEGUI::EventArgs& e);
		
		/// <summary>
		/// Welcome event handlers
		/// </summary>
		class Welcome
		{
		public:
			static bool OnSelectClicked(const CEGUI::EventArgs& e);
			static bool OnItemDoubleClick(const CEGUI::EventArgs& e);
			static bool OnAvatarSelectionChanged(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
			static bool OnWindowCloseClick(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// StatusBar event handlers
		/// </summary>
		class StatusBar
		{
		public:
			static bool OnMoodHappyClicked(const CEGUI::EventArgs& e);
			static bool OnMoodNeutralClicked(const CEGUI::EventArgs& e);
			static bool OnMoodSadClicked(const CEGUI::EventArgs& e);
			static bool OnMoodAngryClicked(const CEGUI::EventArgs& e);
			static bool OnSafetyClicked(const CEGUI::EventArgs& e);
			static bool OnPlayersClicked(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// OnlinePlayers event handlers
		/// </summary>
		class OnlinePlayers
		{
		public:
			static bool OnIgnoreSelectStateChanged(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// RoomObjects event handlers
		/// </summary>
		class RoomObjects
		{
		public:
			static bool OnFilterSelectStateChanged(const CEGUI::EventArgs& e);
			static bool OnListSelectionChanged(const CEGUI::EventArgs& e);		
		};

		/// <summary>
		/// Chat event handlers
		/// </summary>
		class Chat
		{
		public:
			static bool OnKeyDown(const CEGUI::EventArgs& e);
			static bool OnThumbTrackStarted(const CEGUI::EventArgs& e);
			static bool OnThumbTrackEnded(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Avatar event handlers
		/// </summary>
		class Avatar
		{
		public:
			static bool OnHeadMouseClick(const CEGUI::EventArgs& e);
			static bool OnBuffMouseClick(const CEGUI::EventArgs& e);
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// ObjectDetails event handlers
		/// </summary>
		class ObjectDetails
		{
		public:
			static bool OnImageMouseWheel(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// PlayerDetails event handlers
		/// </summary>
		class PlayerDetails
		{
		public:
			static bool OnImageMouseWheel(const CEGUI::EventArgs& e);
			static bool OnOKClicked(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Target event handlers
		/// </summary>
		class Target
		{
		public:
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
			static bool OnInspectMouseClick(const CEGUI::EventArgs& e);
			static bool OnAttackMouseClick(const CEGUI::EventArgs& e);
			static bool OnActivateMouseClick(const CEGUI::EventArgs& e);
			static bool OnBuyMouseClick(const CEGUI::EventArgs& e);
			static bool OnTradeMouseClick(const CEGUI::EventArgs& e);
			static bool OnLootMouseClick(const CEGUI::EventArgs& e);
			static bool OnKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// MiniMap event handlers
		/// </summary>
		class MiniMap
		{
		public:
			static bool OnMouseWheel(const CEGUI::EventArgs& e);
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
			static bool OnMouseDoubleClick(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// RoomEnchantments event handlers
		/// </summary>
		class RoomEnchantments
		{
		public:
			static bool OnBuffMouseClick(const CEGUI::EventArgs& e);
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Buy event handlers
		/// </summary>
		class Buy
		{
		public:
			static bool OnItemAmountDeactivated(const CEGUI::EventArgs& e);
			static bool OnListSelectionChanged(const CEGUI::EventArgs& e);
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnOKClicked(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Attributes handlers
		/// </summary>
		class Attributes
		{
		};

		/// <summary>
		/// Skills event handlers
		/// </summary>
		class Skills
		{
		public:
			static bool OnItemClicked(const CEGUI::EventArgs& e);			
		};

		/// <summary>
		/// Spells event handlers
		/// </summary>
		class Spells
		{
		public:
			static bool OnKeyUp(const CEGUI::EventArgs& e);
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnItemDoubleClicked(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Quests event handlers
		/// </summary>
		class Quests
		{
		public:
			static bool OnItemClicked(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Actions handlers
		/// </summary>
		class Actions
		{
		public:
			static bool OnItemDoubleClicked(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Inventory event handlers
		/// </summary>
		class Inventory
		{
		public:
			static bool OnDragStarted(const CEGUI::EventArgs& e);
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnDragEnded(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// MainButtonsLeft event handlers
		/// </summary>
		class MainButtonsLeft
		{
		public:
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// MainButtonsRight event handlers
		/// </summary>
		class MainButtonsRight
		{
		public:
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Amount event handlers
		/// </summary>
		class Amount
		{
		public:
			static bool OnOKClicked(const CEGUI::EventArgs& e);
			static bool OnKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Trade event handlers
		/// </summary>
		class Trade
		{
		public:
			static bool OnOfferClicked(const CEGUI::EventArgs& e);
			static bool OnAcceptClicked(const CEGUI::EventArgs& e);
			static bool OnItemYouClicked(const CEGUI::EventArgs& e);
			static bool OnItemPartnerClicked(const CEGUI::EventArgs& e);
			static bool OnItemYouAmountDeactivated(const CEGUI::EventArgs& e);
			static bool OnListYouItemDropped(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// ActionButtons event handlers
		/// </summary>
		class ActionButtons
		{
		public:
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnItemDropped(const CEGUI::EventArgs& e);
			static bool OnMouseDown(const CEGUI::EventArgs& e);
			static bool OnMouseUp(const CEGUI::EventArgs& e);
			static bool OnDragEnded(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// NewsGroup event handlers
		/// </summary>
		class NewsGroup
		{
		public:
			static bool OnSelectionChanged(const CEGUI::EventArgs& e);
			static bool OnCreateClicked(const CEGUI::EventArgs& e);
			static bool OnRespondClicked(const CEGUI::EventArgs& e);
			static bool OnMailAuthorClicked(const CEGUI::EventArgs& e);
			static bool OnRefreshClicked(const CEGUI::EventArgs& e);
			static bool OnKeyUp(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// NewsGroup event handlers
		/// </summary>
		class NewsGroupCompose
		{
		public:
			static bool OnSendClicked(const CEGUI::EventArgs& e);	
		};

		/// <summary>
		/// Mail event handlers
		/// </summary>
		class Mail
		{
		public:
			static bool OnSelectionChanged(const CEGUI::EventArgs& e);
			static bool OnCreateClicked(const CEGUI::EventArgs& e);
			static bool OnRespondClicked(const CEGUI::EventArgs& e);
			static bool OnRespondAllClicked(const CEGUI::EventArgs& e);
			static bool OnRefreshClicked(const CEGUI::EventArgs& e);
			static bool OnKeyUp(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// MailCompose event handlers
		/// </summary>
		class MailCompose
		{
		public:
			static bool OnSendClicked(const CEGUI::EventArgs& e);	
		};

		/// <summary>
		/// Guild event handlers
		/// </summary>
		class Guild
		{
		public:
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);	
			static bool OnSupportedSelectStateChanged(const CEGUI::EventArgs& e);
			static bool OnRankSelectionChanged(const CEGUI::EventArgs& e);
			static bool OnDiploSelectionChanged(const CEGUI::EventArgs& e);
			static bool OnSetPasswordClicked(const CEGUI::EventArgs& e);
			static bool OnRenounceClicked(const CEGUI::EventArgs& e);
			static bool OnExileClicked(const CEGUI::EventArgs& e);
			static bool OnGuildShieldSettingChanged(const CEGUI::EventArgs& e);
			static bool OnShieldClaimClicked(const CEGUI::EventArgs& e);			
			static bool OnImageMouseWheel(const CEGUI::EventArgs& e);			
		};

		/// <summary>
		/// GuildCreate event handlers
		/// </summary>
		class GuildCreate
		{
		public:
			static bool OnCreateClicked(const CEGUI::EventArgs& e);
			static bool OnSecretGuildSelectChange(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// AvatarCreateWizard event handlers
		/// </summary>
		class AvatarCreateWizard
		{
		public:
			static bool OnTabChanged(const CEGUI::EventArgs& e);
			static bool OnImageMouseWheel(const CEGUI::EventArgs& e);
			static bool OnFaceSettingChanged(const CEGUI::EventArgs& e);
			static bool OnGenderChanged(const CEGUI::EventArgs& e);

			static bool OnProfileChanged(const CEGUI::EventArgs& e);
			static bool OnAttributeMouseMoveClick(const CEGUI::EventArgs& e);
			
			static bool OnSpellDoubleClicked(const CEGUI::EventArgs& e);
			static bool OnSkillDoubleClicked(const CEGUI::EventArgs& e);
			static bool OnSelectedSpellDoubleClicked(const CEGUI::EventArgs& e);
			static bool OnSelectedSkillDoubleClicked(const CEGUI::EventArgs& e);
			
			static bool OnSpellClicked(const CEGUI::EventArgs& e);
			static bool OnSkillClicked(const CEGUI::EventArgs& e);
			static bool OnSelectedSpellClicked(const CEGUI::EventArgs& e);
			static bool OnSelectedSkillClicked(const CEGUI::EventArgs& e);

			static bool OnButtonBackClicked(const CEGUI::EventArgs& e);
			static bool OnButtonNextClicked(const CEGUI::EventArgs& e);

			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// ConfirmPopup handlers
		/// </summary>
		class ConfirmPopup
		{
		public:
			static bool OnYesClicked(const CEGUI::EventArgs& e);
			static bool OnNoClicked(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// ObjectContents event handlers
		/// </summary>
		class ObjectContents
		{
		public:
			static bool OnItemAmountDeactivated(const CEGUI::EventArgs& e);
			static bool OnItemClicked(const CEGUI::EventArgs& e);
			static bool OnGetClicked(const CEGUI::EventArgs& e);
			static bool OnWindowClosed(const CEGUI::EventArgs& e);
			static bool OnWindowKeyUp(const CEGUI::EventArgs& e);
		};

		/// <summary>
		/// Login event handlers
		/// </summary>
		class Login
		{
		public:
			static bool OnServerChanged(const CEGUI::EventArgs& e);
			static bool OnConnectClicked(const CEGUI::EventArgs& e);
			static bool OnOptionsClicked(const CEGUI::EventArgs& e);

			static bool OnUsernameKeyUp(const CEGUI::EventArgs& e);
			static bool OnPasswordKeyUp(const CEGUI::EventArgs& e);			
		};
		
		/// <summary>
		/// Options event handlers
		/// </summary>
		class Options
		{
		public:
			//static bool OnWindowClosed(const CEGUI::EventArgs& e);
			//static bool OnWindowKeyUp(const CEGUI::EventArgs& e);

			static bool OnKeyLearnKeyUp(const CEGUI::EventArgs& e);
			static bool OnKeyLearnButtonClicked(const CEGUI::EventArgs& e);
			static bool OnKeyLearnButtonDeactivated(const CEGUI::EventArgs& e);

			static bool OnCategoryButtonClicked(const CEGUI::EventArgs& e);

			static bool OnDisplayChanged(const CEGUI::EventArgs& e);
			static bool OnResolutionChanged(const CEGUI::EventArgs& e);
			static bool OnWindowModeChanged(const CEGUI::EventArgs& e);
			static bool OnWindowBordersChanged(const CEGUI::EventArgs& e);
			static bool OnVSyncChanged(const CEGUI::EventArgs& e);
			
			static bool OnFSAAChanged(const CEGUI::EventArgs& e);
			static bool OnFilteringChanged(const CEGUI::EventArgs& e);
			static bool OnImageBuilderChanged(const CEGUI::EventArgs& e);
			static bool OnScalingQualityChanged(const CEGUI::EventArgs& e);
			static bool OnTextureQualityChanged(const CEGUI::EventArgs& e);

			static bool OnDisableMipmapsChanged(const CEGUI::EventArgs& e);
			static bool OnDisableNewRoomTexturesChanged(const CEGUI::EventArgs& e);
			static bool OnDisable3DModelsChanged(const CEGUI::EventArgs& e);
			static bool OnDisableNewSkyChanged(const CEGUI::EventArgs& e);
			static bool OnDisableWeatherEffectsChanged(const CEGUI::EventArgs& e);

			static bool OnParticlesChanged(const CEGUI::EventArgs& e);
			static bool OnDecorationChanged(const CEGUI::EventArgs& e);
			static bool OnMusicVolumeChanged(const CEGUI::EventArgs& e);
			static bool OnSoundVolumeChanged(const CEGUI::EventArgs& e);
			static bool OnDisableLoopSoundsChanged(const CEGUI::EventArgs& e);
			static bool OnPreloadChanged(const CEGUI::EventArgs& e);

			static bool OnRightClickActionChanged(const CEGUI::EventArgs& e);
			static bool OnInvertMouseYChanged(const CEGUI::EventArgs& e);
			static bool OnMouseAimSpeedChanged(const CEGUI::EventArgs& e);
			static bool OnKeyRotateSpeedChanged(const CEGUI::EventArgs& e);

			static bool OnAliasAddClicked(const CEGUI::EventArgs& e);
			static bool OnAliasDeleteClicked(const CEGUI::EventArgs& e);
			static bool OnAliasKeyAccepted(const CEGUI::EventArgs& e);
			static bool OnAliasValueAccepted(const CEGUI::EventArgs& e);
		};
	};
#pragma endregion
};};

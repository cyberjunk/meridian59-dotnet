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

using Android.Widget;
using Android.Views;
using Android.App;
using System.ComponentModel;
using Meridian59.Data.Models;
using Meridian59.Data.Lists;

namespace Meridian59.Android.ChatClient.Adapters
{
    /// <summary>
    /// Adapter for OnlinePlayerList
    /// </summary>
    public class OnlinePlayersAdapter : BaseAdapter<OnlinePlayer>
    {
        protected Activity context;
        protected readonly OnlinePlayerList onlinePlayers;

        public OnlinePlayersAdapter(OnlinePlayerList OnlinePlayers, Activity Activity)
            : base()
        {
            onlinePlayers = OnlinePlayers;
            context = Activity;

            onlinePlayers.ListChanged += chat_ListChanged;
        }

        public override OnlinePlayer this[int position]
        {
            get { return onlinePlayers[position]; }
        }

        public override int Count
        {
            get { return onlinePlayers.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            OnlinePlayer item = onlinePlayers[position];
            View view = convertView;
            
            if (convertView == null || !(convertView is LinearLayout))
                view = context.LayoutInflater.Inflate(Resource.Layout.ChatItemView, parent, false);

            TextView text = view.FindViewById<TextView>(Resource.Id.textItem);
            text.Text = item.Name;

            return view;
        }

        protected void chat_ListChanged(object sender, ListChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }
    }
}

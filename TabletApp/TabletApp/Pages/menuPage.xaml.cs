﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TabletApp;
using TabletApp.Models;
using TabletApp.Models.ServiceRequests;

namespace TabletApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class menuPage : ContentPage
	{
		public menuPage ()
		{
			InitializeComponent ();

            RequestMenuItems();

            if (RealmManager.All<OrderList>().Count() == 0) RealmManager.AddOrUpdate<OrderList>(new OrderList());

            mpViewOrderButton.Clicked += mpViewOrderButton_Clicked;
        }

        async void RequestMenuItems()
        {
            await GetMenuItemsRequest.SendGetMenuItemsRequest();
            SetupMenuItems();
        }

        void SetupMenuItems()
        {
            for (int i = 0; i < RealmManager.All<MenuList>().First().menuItems.Count(); i++)
            {
                Models.MenuItem currItem = RealmManager.All<MenuList>().First().menuItems[i];

                Button newButton = new Button()
                {
                    Text = currItem.name,
                    Margin = new Thickness(30, 0, 30, 15),
                    FontSize = 20,
                    WidthRequest = 100,
                    TextColor = Color.Black
                };

                newButton.Clicked += async (sender, args) => await Navigation.PushAsync(new menuItemPage(currItem));

                if (currItem.category == "Entree") entreeScroll.Children.Add(newButton);
                else if (currItem.category == "Drink") drinkScroll.Children.Add(newButton);
                else if (currItem.category == "Side") sideScroll.Children.Add(newButton);
            }
        }

        private async void mpViewOrderButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new fullOrderPage(RealmManager.All<OrderList>().First()));
        }
    }
}
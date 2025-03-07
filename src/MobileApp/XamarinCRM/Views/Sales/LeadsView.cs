﻿//
//  Copyright 2015  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using Xamarin.Forms;
using XamarinCRM.Layouts;
using XamarinCRM.Statics;
using XamarinCRM.Views.Base;
using XamarinCRM.Views.Sales;
using XamarinCRM.Converters;
using XamarinCRM.Models;

namespace XamarinCRM
{
    public class LeadsView : ModelBoundContentView<SalesDashboardLeadsViewModel>
    {
        public LeadsView()
        {
            #region leads list activity inidicator
            ActivityIndicator leadListActivityIndicator = new ActivityIndicator()
            { 
                HeightRequest = Sizes.MediumRowHeight
            };
            leadListActivityIndicator.SetBinding(IsEnabledProperty, "IsBusy");
            leadListActivityIndicator.SetBinding(IsVisibleProperty, "IsBusy");
            leadListActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            #endregion

            #region loading label
            Label loadingLabel = new Label()
            {
                Text = TextResources.SalesDashboard_Leads_LoadingLabel,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HeightRequest = Sizes.MediumRowHeight,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.End,
                TextColor = Palette._007
            };
            loadingLabel.SetBinding(IsEnabledProperty, "IsBusy");
            loadingLabel.SetBinding(IsVisibleProperty, "IsBusy");
            #endregion

            #region leads list header
            // LeadListHeaderView is an example of a custom view composed with Xamarin.Forms.
            // It takes an action as a constructor parameter, which will be used by the add new lead button ("+").
            LeadListHeaderView leadListHeaderView = new LeadListHeaderView(new Command(ExecutePushLeadDetailsTabbedPageCommand));
            leadListHeaderView.SetBinding(IsEnabledProperty, "IsBusy", converter: new InverseBooleanConverter());
            leadListHeaderView.SetBinding(IsVisibleProperty, "IsBusy", converter: new InverseBooleanConverter());
            #endregion

            #region leadsListView
            LeadListView leadListView = new LeadListView();
            leadListView.SetBinding(LeadListView.ItemsSourceProperty, "Leads");
            leadListView.SetBinding(IsEnabledProperty, "IsBusy", converter: new InverseBooleanConverter());
            leadListView.SetBinding(IsVisibleProperty, "IsBusy", converter: new InverseBooleanConverter());

            leadListView.ItemTapped += (sender, e) =>
            {
                Account leadListItem = (Account)e.Item;
                ExecutePushLeadDetailsTabbedPageCommand(leadListItem);
            };
            #endregion

            #region compose view hierarchy
            Content = new UnspacedStackLayout()
            {
                Children =
                {
                    leadListHeaderView,
                    loadingLabel,
                    leadListActivityIndicator,
                    leadListView
                }
            };
            #endregion
        }

        /// <summary>
        /// We encapsulate ViewModel.PushLeadDetailsTabbedPageCommand.Execute(account) because ViewModel is null during construction of this class.
        /// </summary>
        /// <param name="account">An object of type <see cref="XamarinCRM.Models.Account"/>. Null by default. If null, pushes a fresh lead details tabbed page. If not null, loads the account in the pushed lead details tabbed page.</param>
        void ExecutePushLeadDetailsTabbedPageCommand(object account = null)
        {
            ViewModel.PushLeadDetailsTabbedPageCommand.Execute(account);
        }
    }
}


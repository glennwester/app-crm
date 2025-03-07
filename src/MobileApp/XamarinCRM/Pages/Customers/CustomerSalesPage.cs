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
using Xamarin;
using Xamarin.Forms;
using XamarinCRM.Layouts;
using XamarinCRM.Pages.Base;
using XamarinCRM.Statics;
using XamarinCRM.Views.Customers;
using XamarinCRM.Views.Custom;

namespace XamarinCRM.Pages.Customers
{
    public class CustomerSalesPage : ModelBoundContentPage<CustomerSalesViewModel>
    {
        public CustomerSalesPage()
        {
            BackgroundColor = Color.Transparent;
            #region header
            Label companyTitleLabel = new Label()
            {
                Text = TextResources.Customers_Orders_EditOrder_CompanyTitle,
                TextColor = Palette._007,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                XAlign = TextAlignment.Start,
                YAlign = TextAlignment.End,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            Label companyNameLabel = new Label()
            {
                TextColor = Device.OnPlatform(Palette._006, Color.White, Color.White),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.Start,
                YAlign = TextAlignment.Start,
                LineBreakMode = LineBreakMode.TailTruncation
            };
            companyNameLabel.SetBinding(Label.TextProperty, "Account.Company");

            RelativeLayout headerLabelsRelativeLayout = new RelativeLayout() { HeightRequest = Sizes.LargeRowHeight };

            headerLabelsRelativeLayout.Children.Add(
                view: companyTitleLabel,
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height / 2));

            headerLabelsRelativeLayout.Children.Add(
                view: companyNameLabel,
                yConstraint: Constraint.RelativeToParent(parent => parent.Height / 2),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height / 2));

            ContentView headerLabelsView = new ContentView() { Padding = new Thickness(20, 0), Content = headerLabelsRelativeLayout };
            #endregion

            #region weekly sales chart
            CustomerWeeklySalesChartView customerWeeklySalesChartView = new CustomerWeeklySalesChartView();
            #endregion

            #region category sales chart
            CustomerCategorySalesChartView customerCategorySalesChartView = new CustomerCategorySalesChartView();
            #endregion

            #region platform adjustments
            Device.OnPlatform(
                Android: () =>
                {
                    BackgroundColor = Palette._009;
                }
            );
            #endregion

            #region compose view hierarchy
            Content = new ScrollView()
            { 
                Content = new UnspacedStackLayout()
                {
                    Children =
                    {
                        new ContentViewWithBottomBorder(){ Content = headerLabelsView },
                        customerWeeklySalesChartView,
                        customerCategorySalesChartView
                    }
                }
            };
            #endregion
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.IsInitialized)
            {
                await ViewModel.ExecuteLoadSeedDataCommand(ViewModel.Account);
                ViewModel.IsInitialized = true;
            }

            Insights.Track(InsightsReportingConstants.PAGE_CUSTOMERSALES);
        }
    }
}


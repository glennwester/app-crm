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
using XamarinCRM.Statics;
using XamarinCRM.Models;

namespace XamarinCRM.Views.Products
{
    public class ProductDetailRibbonView : ContentView
    {
        readonly Product _CatalogProduct;

        public ProductDetailRibbonView(Product catalogProduct, bool isPerformingProductSelection = false)
        {
            _CatalogProduct = catalogProduct;

            RelativeLayout relativeLayout = new RelativeLayout();

            BackgroundColor = Palette._009;

            HeightRequest = 20;

            Padding = new Thickness(20, 15);

            if (isPerformingProductSelection)
            {
                Image addToOrderImage = new Image()
                {
                    Aspect = Aspect.AspectFit
                };
                Device.OnPlatform(
                    iOS: () => addToOrderImage.Source = new FileImageSource(){ File = "add_ios_blue" },     
                    Android: () => addToOrderImage.Source = new FileImageSource() { File = "add_android_blue" }
                );

                Label addToOrderTextLabel = new Label()
                {
                    Text = TextResources.Customers_Orders_EditOrder_AddToOrder.ToUpper(),
                    TextColor = Palette._004,
                    XAlign = TextAlignment.Start,
                    YAlign = TextAlignment.Center,
                };

                TapGestureRecognizer addToOrderTapGestureRecognizer = new TapGestureRecognizer()
                { 
                    NumberOfTapsRequired = 1,
                    Command = new Command(AddToOrderTapped)
                };

                addToOrderImage.GestureRecognizers.Add(addToOrderTapGestureRecognizer);
                addToOrderTextLabel.GestureRecognizers.Add(addToOrderTapGestureRecognizer);

                const double imagePaddingPercent = .10;

                relativeLayout.Children.Add(
                    view: addToOrderImage,
                    yConstraint: Constraint.RelativeToParent(parent => parent.Height * imagePaddingPercent),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Height - (parent.Height * imagePaddingPercent * 2)),
                    heightConstraint: Constraint.RelativeToParent(parent => parent.Height - (parent.Height * imagePaddingPercent * 2)));

                relativeLayout.Children.Add(
                    view: addToOrderTextLabel,
                    xConstraint: Constraint.RelativeToView(addToOrderImage, (parent, view) => view.X + view.Width + parent.Height * imagePaddingPercent),
                    widthConstraint: Constraint.RelativeToView(addToOrderImage, (parent, view) => parent.Width - view.Width),
                    heightConstraint: Constraint.RelativeToParent(parent => parent.Height)
                );
            }

            Label priceValueLabel = new Label()
            {
                Text = string.Format("{0:C}", _CatalogProduct.Price),
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Center
            };

            relativeLayout.Children.Add(
                view: priceValueLabel, 
                xConstraint: Constraint.RelativeToParent(parent => parent.Width * .75), 
                yConstraint: Constraint.RelativeToParent(parent => 0), 
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width * .25), 
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height));

            Content = relativeLayout;
        }

        async void AddToOrderTapped()
        {
            MessagingCenter.Send(_CatalogProduct, MessagingServiceConstants.UPDATE_ORDER_PRODUCT);
            await Navigation.PopModalAsync();
            
        }
    }
}



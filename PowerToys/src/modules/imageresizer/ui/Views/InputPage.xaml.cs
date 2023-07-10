﻿// Copyright (c) Brice Lambson
// The Brice Lambson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.  Code forked from Brice Lambson's https://github.com/bricelam/ImageResizer/

using System.Windows.Controls;
using System.Windows.Input;
using ImageResizer.ViewModels;
using ModernWpf.Controls;
using static ImageResizer.ViewModels.InputViewModel;

namespace ImageResizer.Views
{
    public partial class InputPage : UserControl
    {
        public InputPage()
        {
            InitializeComponent();

            // 汉化默认选项，在列表加载完之后
            FitTranslator.Loaded += (s, e) =>
            {
                FitTranslator.SelectedIndex = 1; // 默认选中“缩放”
            };

            // 汉化默认选项，在列表加载完之后
            UnitTranslator.Loaded += (s, e) =>
            {
                UnitTranslator.SelectedIndex = 3; // 默认选中“像素”
            };
        }

        /// <summary>
        /// Pressing Enter key doesn't update value. PropertyChanged is only updated after losing focus to NumberBox.
        /// We add this workaround the UI limitations and might need to be revisited or not needed anymore if we upgrade to WinUI3.
        /// This function handles the KeyDown event for a NumberBox control.
        /// It checks if the key pressed is 'Enter'.
        /// According to the NumberBox name, it creates an instance of the KeyPressParams class with the appropriate dimension (Width or Height) and the parsed double value.
        /// </summary>
        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the key pressed is the 'Enter' key
            if (e.Key == Key.Enter)
            {
                var numberBox = sender as NumberBox;
                var viewModel = (InputViewModel)this.DataContext;
                double number;
                KeyPressParams keyParams;
                if (double.TryParse(((TextBox)e.OriginalSource).Text, out number))
                {
                    // Determine which NumberBox triggered the event based on its name
                    switch (numberBox.Name)
                    {
                        case "WidthNumberBox":
                            keyParams = new KeyPressParams
                            {
                                Value = number,
                                Dimension = Dimension.Width,
                            };
                            break;

                        case "HeightNumberBox":
                            keyParams = new KeyPressParams
                            {
                                Value = number,
                                Dimension = Dimension.Height,
                            };
                            break;

                        default:
                            // Return without EnterKeyPressedCommand executed
                            return;
                    }

                    viewModel.EnterKeyPressedCommand.Execute(keyParams);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Code777
{
    public partial class CardManipulationxaml : PhoneApplicationPage
    {
        public CardManipulationxaml()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //no longer register for navigation chaned
            Messenger.Default.Unregister<DialogMessage>(this);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            Messenger.Default.Register<DialogMessage>(
                this,
                msg =>
                {
                    var result = MessageBox.Show(
                        msg.Content,
                        msg.Caption,
                        msg.Button);

                    // Send callback
                    msg.ProcessCallback(result);

                });
            base.OnNavigatedTo(e);
        }
    }



}
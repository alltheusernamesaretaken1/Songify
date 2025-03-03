﻿using Songify_Slim.Models;
using Songify_Slim.Util.Songify;
using System.Windows;

namespace Songify_Slim
{
    /// <summary>
    ///     Queue Window to display the current song queue
    /// </summary>
    public partial class Window_Queue
    {
        public Window_Queue()
        {
            InitializeComponent();
        }

        // This window shows the current Queue in a DataGrid
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // This loads in all the requestobjects
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() != typeof(MainWindow))
                    continue;

                dgv_Queue.ItemsSource = (window as MainWindow)?.ReqList;
            }
        }

        private void DgvItemDelete_Click(object sender, RoutedEventArgs e)
        {
            // This deletes the selected requestobject
            if (dgv_Queue.SelectedItem == null)
                return;

            RequestObject req = (RequestObject)dgv_Queue.SelectedItem;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() != typeof(MainWindow))
                    continue;
                (window as MainWindow)?.ReqList.Remove(req);
            }

            WebHelper.UpdateWebQueue(req.TrackID, "", "", "", "", "1", "u");
            dgv_Queue.Items.Refresh();
        }
    }
}
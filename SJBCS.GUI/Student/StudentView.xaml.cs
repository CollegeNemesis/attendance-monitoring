﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SJBCS.GUI.Student
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : UserControl
    {
        public StudentView()
        {
            InitializeComponent();
        }

        //private void CompletedJobsMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    StudentViewModel viewModel = (StudentViewModel)DataContext;
        //    if (viewModel.RowDetailsVisible == DataGridRowDetailsVisibilityMode.Collapsed)
        //    {
        //        viewModel.RowDetailsVisible = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        //    }
        //    else
        //    {
        //        viewModel.RowDetailsVisible = DataGridRowDetailsVisibilityMode.Collapsed;
        //    }
        //}
    }
}

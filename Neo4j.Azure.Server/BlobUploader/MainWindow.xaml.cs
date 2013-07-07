using System;
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
using BlobUploader.Infrastructure;
using Neo4jUploader;

namespace BlobUploader {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {
      public MainWindow() {
         CloudUploader = new CloudUploader();
         ApplicationSettings = new ApplicationSettings();

         UseLocalStorage = ApplicationSettings.UseLocalStorage;
         UseCloudStorage = !UseLocalStorage;

         InitializeComponent();
      }

      public ApplicationSettings ApplicationSettings   { get; set; }
      public CloudUploader CloudUploader { get; set; }

      public bool UseLocalStorage {
         get { return (bool)GetValue(UseLocalStorageProperty); }
         set { SetValue(UseLocalStorageProperty, value); }
      }

      // Using a DependencyProperty as the backing store for UseLocalStorage.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty UseLocalStorageProperty =
          DependencyProperty.Register("UseLocalStorage", typeof(bool), typeof(MainWindow), new PropertyMetadata(false, UpdateLocalStorageSettings));

      private static void UpdateLocalStorageSettings(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var mainWindow = (MainWindow)d;
         mainWindow.UseCloudStorage = !mainWindow.UseLocalStorage;
         mainWindow.UpdateLocationSettings();
      }

      public bool UseCloudStorage {
         get { return (bool)GetValue(UseCloudStorageProperty); }
         set { SetValue(UseCloudStorageProperty, value); }
      }

      // Using a DependencyProperty as the backing store for UseCloudStorage.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty UseCloudStorageProperty =
          DependencyProperty.Register("UseCloudStorage", typeof(bool), typeof(MainWindow), new PropertyMetadata(false, UpdateCloudStorageSettings));

      private static void UpdateCloudStorageSettings(DependencyObject d, DependencyPropertyChangedEventArgs e) {
         var mainWindow = (MainWindow)d;
         mainWindow.UseLocalStorage = !mainWindow.UseCloudStorage;
         mainWindow.UpdateLocationSettings();
      }

      private void UpdateLocationSettings() {
         ApplicationSettings.UseLocalStorage = UseLocalStorage;
      }
   }
}

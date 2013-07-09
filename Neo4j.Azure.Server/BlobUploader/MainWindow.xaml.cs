using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

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

      private void JavaPathButtonClick(object sender, RoutedEventArgs e) {
         ApplicationSettings.JavaLocation = GetFilePath(ApplicationSettings.JavaLocation);
      }

      private void Neo4JPathButtonClick(object sender, RoutedEventArgs e) {
         ApplicationSettings.Neo4jLocation = GetFilePath(ApplicationSettings.Neo4jLocation);
      }

      private String GetFilePath(String existingfilePath) {
         // Create OpenFileDialog 
         var dlg = new Microsoft.Win32.OpenFileDialog {
            DefaultExt = ".zip",
            Filter =
               "Zip Files |*.zip"
         };

         var result = dlg.ShowDialog();

         // Get the selected file name and display in a TextBox 
         if (result == true) {
            // Open document 
            return dlg.FileName;
         }
         else {
            return existingfilePath;
         }
      }

      private void UploadNeo4JCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
         if (String.IsNullOrEmpty(ApplicationSettings.Neo4jLocation)) { }
         else if (String.IsNullOrEmpty(ApplicationSettings.AccountName)) { }
         else if (String.IsNullOrEmpty(ApplicationSettings.AccountKey)) { }
         else {
            e.CanExecute = true;
         }
      }

      private void UploadNeo4JCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
         InitialiseCloudStorage();
         UploadFile(ApplicationSettings.Neo4jLocation);
      }


      private void UploadJavaCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
         if (String.IsNullOrEmpty(ApplicationSettings.JavaLocation)) { }
         else if (String.IsNullOrEmpty(ApplicationSettings.AccountName)) { }
         else if (String.IsNullOrEmpty(ApplicationSettings.AccountKey)) { }
         else {
            e.CanExecute = true;
         }
      }

      private void UploadJavaCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
         InitialiseCloudStorage();
         UploadFile(ApplicationSettings.JavaLocation);
      }

      private void InitialiseCloudStorage() {
         if (this.UseCloudStorage) {
            CloudUploader.InitialiseCloudBlobClient("https", ApplicationSettings.AccountName, ApplicationSettings.AccountKey);
         }
         else {
            CloudUploader.InitialiseLocalBlobClient();
         }
      }

      private void UploadFile(string fileLocation) {
         var fileName = Path.GetFileName(fileLocation);
         CloudUploader.Upload(fileName, fileLocation);
      }
   }
}

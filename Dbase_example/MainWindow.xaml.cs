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

namespace Dbase_example
{
    /// <summary>
    /// Author:     Christopher Whitehead<cwhitehead73@gmail.com>
    /// Purpose:    C# Dbase Example using "Chinook SQLite DB" with different types of SQL Queries
    ///             SQLite backend used as DB for portability and availability(no DB Server Setup)               
    ///             This DOES NOT incorporate complete Error Checking
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLConnector mLibrary;
        private List<ArtistData> mArtists;
        private List<AlbumData> mAlbums;
        private List<TrackData> mTracks;

        public MainWindow()
        {
            InitializeComponent();
            this.mLibrary = new SQLConnector();
            mArtists = this.mLibrary.getArtists();
            xArtists.ItemsSource = this.mArtists;
            this.mAlbums = this.mLibrary.getAlbums(this.mArtists[0].ArtistId);            
            this.mTracks = this.mLibrary.getTracks(this.mAlbums[0].AlbumId);
            xAlbums.ItemsSource = this.mAlbums;
            xGridTracks.ItemsSource = this.mTracks;
        }
        private void xExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void xArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                      
            int aData = ((sender as DataGrid).CurrentItem as ArtistData).ArtistId;
            this.mAlbums = this.mLibrary.getAlbums(aData);
            xAlbums.ItemsSource = this.mAlbums;
            if(this.mAlbums.Count > 0)
            {
                xGridTracks.ItemsSource = this.mLibrary.getTracks(this.mAlbums[0].AlbumId);
            }
            else
            {
                xGridTracks.ItemsSource = null;
            }    

        }

        private void xAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int aData = ((sender as DataGrid).CurrentItem as AlbumData).AlbumId;
                xGridTracks.ItemsSource = this.mLibrary.getTracks(aData);
            }
            catch(System.NullReferenceException ex)
            {
                xGridTracks.ItemsSource = null;
            }
           
            
        }
    }
}

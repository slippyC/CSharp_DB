using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


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
        private ObservableCollection<ArtistData> mArtists;
        private List<AlbumData> mAlbums;
        private List<TrackData> mTracks;

        public MainWindow()
        {
            InitializeComponent();
            this.mLibrary = new SQLConnector();
            mArtists = new ObservableCollection<ArtistData>(this.mLibrary.getArtists());
            // Event raised when collection changes
            mArtists.CollectionChanged += collectionChanged;
            xArtists.ItemsSource = this.mArtists;
            this.mAlbums = this.mLibrary.getAlbums(this.mArtists[0].ArtistId);            
            this.mTracks = this.mLibrary.getTracks(this.mAlbums[0].AlbumId);
            xAlbums.ItemsSource = this.mAlbums;
            xGridTracks.ItemsSource = this.mTracks;
            setAlbumArt();
            
        }
        private void xExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void xArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                      
            artistChanged(((sender as DataGrid).CurrentItem as ArtistData).ArtistId);            
        }

        private void artistChanged(int ArtistId)
        {
            xAlbumPic.Source = null;
            this.mAlbums = this.mLibrary.getAlbums(ArtistId);
            xAlbums.ItemsSource = this.mAlbums;
            if (this.mAlbums.Count > 0)
            {
                xGridTracks.ItemsSource = this.mLibrary.getTracks(this.mAlbums[0].AlbumId);
                setAlbumArt();
            }
            else
            {
                xGridTracks.ItemsSource = null;
            }
        }

        private void xAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setAlbumArt();
        }

        private async void setAlbumArt()
        {
            xAlbumPic.Source = null;
            if (mAlbums.Count < 1)
                return;

            string artist = "";
            if (xArtists.SelectedIndex == -1)
                artist = mArtists[0].Name;
            else
                artist = (xArtists.SelectedItem as ArtistData).Name;

            string album = "";
            if (xAlbums.SelectedIndex == -1)
                album = mAlbums[0].Title;
            else
                album = (xAlbums.SelectedItem as AlbumData).Title;

            AlbumArt art = new AlbumArt();
            Task<BitmapImage> t = new Task<BitmapImage>(() => { return art.getFrontCover(artist, album); });
            t.Start();
            BitmapImage bm = await t;
            if (bm != null)
                xAlbumPic.Source = bm;
        }      

        // EventHandler for mArtists to update xArtists when the collection changes
        private void collectionChanged(object sender,NotifyCollectionChangedEventArgs e)
        {
            xArtists.ItemsSource = this.mArtists;            
        }

        // Filter Artists
        private void xFilter_KeyUp(object sender, KeyEventArgs e)
        {
            xAlbumPic.Source = null;
            mArtists = new ObservableCollection<ArtistData>(mLibrary.getArtists(xFilter.Text + '%'));
            mArtists.CollectionChanged += collectionChanged;
            xArtists.ItemsSource = mArtists;
            if(mArtists.Count > 0)
                artistChanged(mArtists[0].ArtistId);
            else
            {                
                xAlbums.ItemsSource = null;
                xGridTracks.ItemsSource = null;
            }           
        }
    }
}

//------------------------------------------------------------
// <copyright file="SettingsModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System;
    using System.Collections.ObjectModel;
    using Windows.UI.Xaml.Controls.Maps;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using System.Threading.Tasks;

    /// <summary>
    /// Icons: https://msdn.microsoft.com/en-us/library/windows/apps/jj841126.aspx
    /// </summary>
    public class SettingsModel : BaseSettingsPage<AppLevelModel>
    {
        /// <summary>
        /// Not used
        /// </summary>
        public override string SearchTerm { get; set; }

        public SettingsModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            AboutViewModel = new AboutModel(appViewModel);
            MainMenu = appViewModel.TopLevelMenu;
            MainMenuIndex = 3;

            IconCode = "Setting";
            PageTitle = Q.Resources.Settings_PageTitle;

            MapStyleNames = new string[]
            {
                "None",
                "Road",
                "Aerial",
                "Aerial3D",
                "Aerial3DWithRoads",
                "AerialWithRoads",
                "Terrain"
            };

            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(AboutViewModel, typeof(AboutPage) )
            };
            BottomMenuIndex = -1;
            ResetBottomMenuIndexOnNavigation = true;

            MapStyles = new MapStyle[]
            {
                MapStyle.None,
                MapStyle.Road,
                MapStyle.Aerial,
                MapStyle.Aerial3D,
                MapStyle.Aerial3DWithRoads,
                MapStyle.AerialWithRoads,
                MapStyle.Terrain
            };
        }

        public AboutModel AboutViewModel
        {
            get
            {
                return GetState<AboutModel>();
            }

            private set
            {
                SetState(value);
            }
        }

        public string[] TimeList
        {
            get
            {
                return GetState(
                    new string[]
                    {
                        "12:00AM", "12:30AM", "1:00AM", "1:30AM","2:00AM", "2:30AM",
                        "3:00AM", "3:30AM", "4:00AM", "4:30AM", "5:00AM", "5:30AM",
                        "6:00AM", "6:30AM", "7:00AM", "7:30AM", "8:00AM", "8:30AM",
                        "9:00AM", "9:30AM", "10:00AM", "10:30AM","11:00AM", "11:30AM",
                        "12:00PM", "12:30PM", "1:00PM", "1:30PM", "2:00PM", "2:30PM",
                        "3:00PM", "3:30PM", "4:00PM", "4:30PM", "5:00PM", "5:30PM",
                        "6:00PM", "6:30PM", "7:00PM", "7:30PM", "8:00PM", "8:30PM",
                        "9:00PM", "9:30PM", "10:00PM", "10:30PM", "11:00PM", "11:30PM"
                    });
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Designed to translate time stamp from the <code>TimeList</code> format
        /// </summary>
        public TimeSpan GetTimespan(string timeString)
        {
            var colonIndex = timeString.IndexOf(':');

            var hours = int.Parse(timeString.Substring(0, colonIndex));
            var ofset = timeString.Substring(timeString.Length - 2, 2).ToLower() == "am" ? 0 : 12;
            hours = (hours == 12 ? 0 : hours) + ofset;

            var minutes = int.Parse(
                timeString.Substring(colonIndex + 1, timeString.Length - colonIndex - 3));

            return new TimeSpan(hours, minutes, 0);
        }

        public ObservableCollection<string> EventImageList
        {
            get
            {
                return GetState
                (
                    new ObservableCollection<string>()
                    {
                        "ms-appx:///Assets/Images/Events/Meeting/1431399162-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/Calendar.png",
                        "ms-appx:///Assets/Images/Events/Meeting/EventGroup.png",
                        "ms-appx:///Assets/Images/Events/Meeting/Interview-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/meeting-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/NewMeeting.png",
                        "ms-appx:///Assets/Images/Events/Meeting/office-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/penguinadmin-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/podium-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/desenhocs-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/empty-calendar-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/GS-Journal-100px.png",
                        "ms-appx:///Assets/Images/Events/Meeting/new-year03-100px.png",

                        "ms-appx:///Assets/Images/Events/General/Anonymous-Crown-100px.png",
                        "ms-appx:///Assets/Images/Events/General/Community-by-Merlin2525-100px.png",
                        "ms-appx:///Assets/Images/Events/General/Happy-Shopping-Girl-100px.png",
                        "ms-appx:///Assets/Images/Events/General/littlestorefront-100px.png",
                        "ms-appx:///Assets/Images/Events/General/ryanlerch-deck-of-cards-100px.png",
                        "ms-appx:///Assets/Images/Events/General/Summer-2010-ClipArt7-HotDog-100px.png",

                        "ms-appx:///Assets/Images/Events/Party/Anonymous-Cake-1-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/Balloon-Horse-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/Champagne-Showers-1-by-Merlin2525-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/CHRISTMAS-TREE-2-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/Disco-ball-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/Distinguished-Gentleman-Dancing-by-Merlin2525-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/firecrackers-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/giardinaggio-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/Halloween-Baby-Costume-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/I-Love-You-Valentine-by-Merlin2525-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/johnny-automatic-champagne-on-ice-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/nicubunu-Chocolate-birthday-cake-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/pace-e-bene--architetto--01-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/partyhat-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/snowman-glossy-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/tom-Venetian-mask-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/trophy-100px.png",

                        "ms-appx:///Assets/Images/Events/Party/chinese-new-year01-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/cinco-mayo01-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/father-day02-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/Gerald-G-Campfires-and-cooking-cranes-5-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/graduation02-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/haft-seen-table-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/hanukkah01-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/jonata-Separation-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/ludo-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/mother-day01-100px.png",
                        "ms-appx:///Assets/Images/Events/Party/thanksgiving03-100px.png",

                        "ms-appx:///Assets/Images/Events/Sports/Golf-Course-Sports2010-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/Happy-Camping-Archery-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/johnny-automatic-girl-playing-soccer-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/redblue-convertible-sports-car-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/skatingpanda-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/SnarkHunter-Arrow-in-the-gold-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/sport-3-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/sport-4-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/sport-5-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/sports-fan-100px.png",
                        "ms-appx:///Assets/Images/Events/Sports/talkingsnowboardgirl-100px.png",
                    }
                );
            }
        }

        public ObservableCollection<string> GuestImageList_Business
        {
            get
            {
                return GetState
                (
                    new ObservableCollection<string>()
                    {
                        "ms-appx:///Assets/Images/Guests/Business/1378052551-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/1378052687-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/3D-Glasses-4-by-Merlin2525-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/big-wave-hello-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/cooking-man-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/deep-in-thought-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/little-wave-hello-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/man-chris-kempson-01-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/msewtz-Business-Person-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person3-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person4-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person5-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person6-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person7-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person8-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/person9-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/pose-oh-crap-what-did-i-just-do2-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/side-face1-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/side-face2-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/therapist-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-female-cyan-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-female-pink-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-female-sad-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-female-smile-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-male-blue-4-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-male-pink-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-male-sad-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz-male-smile-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz2-male-sad-100px.png",
                        "ms-appx:///Assets/Images/Guests/Business/yyycatch-people-biz2-male-smile-100px.png",

                        "ms-appx:///Assets/Images/Guests/Cartoon/1368500161-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570104-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570105-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570106-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570107-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570108-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570109-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570110-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570111-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570112-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570113-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570115-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570116-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570117-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570118-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570119-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570122-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570123-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570124-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570125-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570126-100px.png",
                        "ms-appx:///Assets/Images/Guests/Cartoon/570127-100px.png",
                    }
                );
            }
        }

        public string UserAvatarImage
        {
            get
            {
                return GetState(SqLiteManager.DefaultContactImagePath, SaveType.RoamingSettings);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ms-appx:///"))
                {
                    SetState(value, SaveType.RoamingSettings);
                }
                else
                {
                    SetState("ms-appx:///" + value, SaveType.RoamingSettings);
                }
            }
        }

        public string UserName
        {
            get
            {
                return GetState("Not Logged In", SaveType.RoamingSettings);
            }
            set
            {
                SetState(value, SaveType.RoamingSettings);
            }
        }

        public static ImageSource HappyFace =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/UI/smiley-icon.png"));
        public static ImageSource SadFace =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/UI/smiley-sad-icon.png"));

        public static ImageSource Arrived =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/Attendance/Arrived.png"));
        public static ImageSource ArrivedLate =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/Attendance/ArrivedLate.png"));
        public static ImageSource DidNotCome =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/Attendance/DidNotCome.png"));
        public static ImageSource UnknownArrival =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/Attendance/Unknown.png"));

        public static ImageSource TakeAttendance =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/PeopleGroups/mix-and-match-characters-show-off-100px.png"));
        public static ImageSource EditAttendance =
            new BitmapImage(new Uri("ms-appx:///Assets/Images/PeopleGroups/nicubunu-Abstract-people-100px.png"));

        /// <summary>
        /// If true, set name as 'Last, First', else 'First last'
        /// </summary>
        public bool? UserNameInStandardFormat
        {
            get
            {
                return GetState(true, SaveType.RoamingSettings);
            }
            set
            {
                if (SetState(value, SaveType.RoamingSettings))
                    AppViewModel.AllContacts.Contacts.UpdateOrdering(value.Value);
            }
        }

        #region Map settings
        public string[] MapStyleNames { get; }

        private MapStyle[] MapStyles { get; }

        public int MapStyleIndex
        {
            get
            {
                return GetState(0, SaveType.RoamingSettings);
            }

            set
            {
                SetState(value, SaveType.RoamingSettings);
                NotifyPropertyUpdated(nameof(CurrentMapStyle));
            }
        }

        public MapStyle CurrentMapStyle
        {
            get
            {
                return MapStyles[MapStyleIndex];
            }
        }

        /// <summary>
        /// Key found here: https://www.bingmapsportal.com/Account
        /// </summary>
        internal string MapServiceToken
        {
            get
            {
                return "AuuvSIjzNja_iwHxEDpqMouGqPEEcC1behb7HrDkH2nCbY_wvXSJGcwksYdCTd7A";
            }
        }

        public bool TrafficFlowVisible
        {
            get
            {
                return GetState(false, SaveType.RoamingSettings);
            }

            set
            {
                SetState(value, SaveType.RoamingSettings);
            }
        }
        #endregion

        public override Task<bool> OnNavigatingFrom()
        {
            return Task.Run(() => { return true; });
        }
    }
}

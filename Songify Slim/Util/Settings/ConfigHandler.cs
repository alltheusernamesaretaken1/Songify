﻿using Songify_Slim.Util.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static System.Convert;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using Application = System.Windows.Application;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Songify_Slim
{
    /// <summary>
    ///     This class is for writing, exporting and importing the config file
    ///     The config file is XML and has a single config tag with attributes
    /// </summary>
    internal class ConfigHandler
    {
        public enum ConfigTypes
        {
            SpotifyCredentials,
            TwitchCredentials,
            BotConfig,
            AppConfig
        }

        public static void WriteConfig(ConfigTypes configType, object o)
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string yaml;

            switch (configType)
            {
                case ConfigTypes.SpotifyCredentials:
                    path += "/SpotifyCredentials.yaml";
                    yaml = serializer.Serialize(o as SpotifyCredentials);
                    break;
                case ConfigTypes.TwitchCredentials:
                    path += "/TwitchCredentials.yaml";
                    yaml = serializer.Serialize(o as TwitchCredentials);
                    break;
                case ConfigTypes.BotConfig:
                    path += "/BotConfig.yaml";
                    yaml = serializer.Serialize(o as BotConfig);
                    break;
                case ConfigTypes.AppConfig:
                    path += "/AppConfig.yaml";
                    yaml = serializer.Serialize(o as AppConfig);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(configType), configType, null);
            }
            File.WriteAllText(path, yaml);
        }

        public static void ReadConfig()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();

            Configuration config = new Configuration();

            foreach (ConfigTypes ConfigType in (ConfigTypes[])Enum.GetValues(typeof(ConfigTypes)))
            {
                switch (ConfigType)
                {
                    case ConfigTypes.SpotifyCredentials:
                        if (File.Exists($@"{path}\SpotifyCredentials.yaml"))
                        {
                            var p = deserializer.Deserialize<SpotifyCredentials>(File.ReadAllText($@"{path}\SpotifyCredentials.yaml"));
                            config.SpotifyCredentials = p;
                        }
                        else
                        {
                            config.SpotifyCredentials = new SpotifyCredentials
                            {
                                AccessToken = Settings.SpotifyAccessToken,
                                RefreshToken = Settings.SpotifyRefreshToken,
                                DeviceId = Settings.SpotifyDeviceId,
                                ClientId = Settings.SpotifyDeviceId,
                                ClientSecret = Settings.ClientSecret
                            };

                        }
                        break;
                    case ConfigTypes.TwitchCredentials:
                        if (File.Exists($@"{path}\TwitchCredentials.yaml"))
                        {
                            var p = deserializer.Deserialize<TwitchCredentials>(File.ReadAllText($@"{path}\TwitchCredentials.yaml"));
                            config.TwitchCredentials = p;
                        }
                        else
                        {
                            config.TwitchCredentials = new TwitchCredentials
                            {
                                AccessToken = "",
                                ChannelName = Settings.TwChannel,
                                ChannelId = "",
                                BotAccountName = Settings.TwAcc,
                                BotOAuthToken = Settings.TwOAuth
                            };
                        }
                        break;
                    case ConfigTypes.BotConfig:
                        if (File.Exists($@"{path}\BotConfig.yaml"))
                        {
                            var p = deserializer.Deserialize<BotConfig>(File.ReadAllText($@"{path}\BotConfig.yaml"));
                            config.BotConfig = p;
                        }
                        else
                        {
                            config.BotConfig = new BotConfig
                            {
                                BotCmdNext = Settings.BotCmdNext,
                                BotCmdPos = Settings.BotCmdPos,
                                BotCmdSkip = Settings.BotCmdSkip,
                                BotCmdSkipVote = Settings.BotCmdSkipVote,
                                BotCmdSong = Settings.BotCmdSong,
                                BotCmdSkipVoteCount = Settings.BotCmdSkipVoteCount,
                                BotRespBlacklist = Settings.BotRespBlacklist,
                                BotRespError = Settings.BotRespError,
                                BotRespIsInQueue = Settings.BotRespIsInQueue,
                                BotRespLength = Settings.BotRespLength,
                                BotRespMaxReq = Settings.BotRespMaxReq,
                                BotRespModSkip = Settings.BotRespModSkip,
                                BotRespNoSong = Settings.BotRespNoSong,
                                BotRespSuccess = Settings.BotRespSuccess,
                                BotRespVoteSkip = Settings.BotRespVoteSkip
                            };
                        }
                        break;
                    case ConfigTypes.AppConfig:
                        if (File.Exists($@"{path}\AppConfig.yaml"))
                        {
                            var p = deserializer.Deserialize<AppConfig>(File.ReadAllText($@"{path}\AppConfig.yaml"));
                            config.AppConfig = p;
                        }
                        else
                        {
                            config.AppConfig = new AppConfig
                            {
                                AnnounceInChat = Settings.AnnounceInChat,
                                AppendSpaces = Settings.AppendSpaces,
                                AutoClearQueue = Settings.AutoClearQueue,
                                Autostart = Settings.Autostart,
                                CustomPauseTextEnabled = Settings.CustomPauseTextEnabled,
                                DownloadCover = Settings.DownloadCover,
                                MsgLoggingEnabled = Settings.MsgLoggingEnabled,
                                OpenQueueOnStartup = Settings.OpenQueueOnStartup,
                                SaveHistory = Settings.SaveHistory,
                                SplitOutput = Settings.SplitOutput,
                                Systray = Settings.Systray,
                                Telemetry = Settings.Telemetry,
                                TwAutoConnect = Settings.TwAutoConnect,
                                TwSrCommand = Settings.TwSrCommand,
                                TwSrReward = Settings.TwSrReward,
                                Upload = Settings.Upload,
                                UploadHistory = Settings.UploadHistory,
                                UseOwnApp = Settings.UseOwnApp,
                                MaxSongLength = Settings.MaxSongLength,
                                PosX = (int)Settings.PosX,
                                PosY = (int)Settings.PosY,
                                SpaceCount = Settings.SpaceCount,
                                TwRewardId = Settings.TwRewardId,
                                TwSrCooldown = Settings.TwSrCooldown,
                                TwSrMaxReq = Settings.TwSrMaxReq,
                                TwSrMaxReqBroadcaster = Settings.TwSrMaxReqBroadcaster,
                                TwSrMaxReqEveryone = Settings.TwSrMaxReqEveryone,
                                TwSrMaxReqModerator = Settings.TwSrMaxReqModerator,
                                TwSrMaxReqSubscriber = Settings.TwSrMaxReqSubscriber,
                                TwSrMaxReqVip = Settings.TwSrMaxReqVip,
                                TwSrUserLevel = Settings.TwSrUserLevel,
                                ArtistBlacklist = Settings.ArtistBlacklist,
                                Color = Settings.Color,
                                CustomPauseText = Settings.CustomPauseText,
                                Directory = Settings.Directory,
                                Language = Settings.Language,
                                OutputString = Settings.OutputString,
                                OutputString2 = Settings.OutputString2,
                                Theme = Settings.Theme,
                                UserBlacklist = Settings.UserBlacklist,
                                Uuid = Settings.Uuid,
                            };
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Settings.Import(config);
        }

        //public static void SaveConfig(string path = "")
        //{
        //    // Saving the Config file
        //    if (path != "")
        //    {
        //        WriteXml(path);
        //    }
        //    else
        //    {
        //        // Importing the SaveFileDialog and giving it filter, directory and window title
        //        SaveFileDialog saveFileDialog = new SaveFileDialog
        //        {
        //            Filter = "XML (*.xml)|*.xml",
        //            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
        //            Title = "Export Config"
        //        };

        //        // Opneing the dialog and if the user clicked on "save" this code gets executed
        //        if (saveFileDialog.ShowDialog() != true) return;
        //        if (saveFileDialog.FileName != "")
        //        {
        //            WriteXml(saveFileDialog.FileName);
        //        }
        //    }
        //}

        //public static void WriteXml(string path, bool hidden = false)
        //{
        //    if (!File.Exists(path))
        //        File.Create(path).Close();
        //    FileInfo myFile = new FileInfo(path);
        //    // Remove the hidden attribute of the file
        //    myFile.Attributes &= ~FileAttributes.Hidden;

        //    // XML-Writer settings
        //    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
        //    {
        //        Indent = true,
        //        IndentChars = "\t",
        //        NewLineOnAttributes = true
        //    };

        //    // Writing the XML, Attributnames are somewhat equal to Settings.
        //    using (XmlWriter writer = XmlWriter.Create(path, xmlWriterSettings))
        //    {
        //        writer.WriteStartDocument();
        //        writer.WriteStartElement("Songify_Config");
        //        writer.WriteStartElement("Config");
        //        Config cfg = Settings.Export();
        //        foreach (PropertyInfo prop in cfg.GetType().GetProperties())
        //        {
        //            writer.WriteAttributeString(prop.Name.ToLower(), prop.GetValue(cfg, null).ToString());
        //        }
        //        writer.WriteEndElement();
        //        writer.WriteEndElement();
        //    }

        //    myFile.Attributes &= ~FileAttributes.Hidden;
        //}

        public static void ReadXml(string path)
        {
            try
            {
                if (new FileInfo(path).Length == 0)
                {
                    //WriteXml(path);
                    return;
                }

                List<string> fileList = new List<string> { "SpotifyCredentials.yaml", "TwitchCredentials.yaml", "BotConfig.yaml", "AppConfig.yaml" };
                if (File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                                @"\SpotifyCredentials.yaml"))
                {
                    fileList.Remove("SpotifyCredentials.yaml");
                }
                if (File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                                @"\TwitchCredentials.yaml"))
                {
                    fileList.Remove("TwitchCredentials.yaml");
                }
                if (File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                                @"\BotConfig.yaml"))
                {
                    fileList.Remove("BotConfig.yaml");
                }
                if (File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                                @"\AppConfig.yaml"))
                {
                    fileList.Remove("AppConfig.yaml");
                }


                foreach (string s in fileList.Where(s => File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + $@"\{s}")))
                {
                    fileList.Remove(s);
                }

                if (fileList.Count == 0)
                {
                    ReadConfig();
                    return;
                }

                // reading the XML file, attributes get saved in Settings
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                if (doc.DocumentElement == null) return;
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.Name != "Config") continue;
                    //Create a new Config object and set the attributes
                    Config config = new Config();
                    if (node.Attributes != null)
                    {
                        int.TryParse(
                            node.Attributes["twsrmaxreq"] != null
                                ? node.Attributes["twsrmaxreq"].InnerText
                                : Settings.TwSrMaxReq.ToString(), out int twsrmaxreq);
                        int.TryParse(
                            node.Attributes["twsrcooldown"] != null
                                ? node.Attributes["twsrcooldown"].InnerText
                                : Settings.TwSrCooldown.ToString(), out int twsrcooldown);

                        config.AccessToken = node.Attributes["accesstoken"] != null ? node.Attributes["accesstoken"].Value : Settings.SpotifyAccessToken;
                        config.AnnounceInChat = ToBoolean(node.Attributes["announceinchat"] != null ? node.Attributes["announceinchat"].Value : Settings.AnnounceInChat.ToString());
                        config.AppendSpaces = ToBoolean(node.Attributes["spacesenabled"] != null ? node.Attributes["spacesenabled"].Value : Settings.AppendSpaces.ToString());
                        config.ArtistBlacklist = node.Attributes["artistblacklist"] != null ? node.Attributes["artistblacklist"].Value : Settings.ArtistBlacklist;
                        config.AutoClearQueue = ToBoolean(node.Attributes["autoclearqueue"] != null ? node.Attributes["autoclearqueue"].Value : Settings.AutoClearQueue.ToString());
                        config.Autostart = ToBoolean(node.Attributes["autostart"] != null ? node.Attributes["autostart"].Value : Settings.Autostart.ToString());
                        config.BotCmdNext = ToBoolean(node.Attributes["botcmdnext"] != null ? node.Attributes["botcmdnext"].Value : Settings.BotCmdNext.ToString());
                        config.BotCmdPos = ToBoolean(node.Attributes["botcmdpos"] != null ? node.Attributes["botcmdpos"].Value : Settings.BotCmdPos.ToString());
                        config.BotCmdSkip = ToBoolean(node.Attributes["botcmdskip"] != null ? node.Attributes["botcmdskip"].Value : Settings.BotCmdSkip.ToString());
                        config.BotCmdSkipVote = ToBoolean(node.Attributes["botcmdskipvote"] != null ? node.Attributes["botcmdskipvote"].Value : Settings.BotCmdSkipVote.ToString());
                        config.BotCmdSkipVoteCount = ToInt32(node.Attributes["botcmdskipvotecount"] != null ? node.Attributes["botcmdskipvotecount"].Value : Settings.BotCmdSkipVoteCount.ToString());
                        config.BotCmdSong = ToBoolean(node.Attributes["botcmdsong"] != null ? node.Attributes["botcmdsong"].Value : Settings.BotCmdSong.ToString());
                        config.BotRespBlacklist = node.Attributes["botrespblacklist"] != null ? node.Attributes["botrespblacklist"].Value : Settings.ClientSecret;
                        config.BotRespError = node.Attributes["botresperror"] != null ? node.Attributes["botresperror"].Value : Settings.BotRespError;
                        config.BotRespIsInQueue = node.Attributes["botrespisinqueue"] != null ? node.Attributes["botrespisinqueue"].Value : Settings.BotRespIsInQueue;
                        config.BotRespLength = node.Attributes["botresplength"] != null ? node.Attributes["botresplength"].Value : Settings.BotRespLength;
                        config.BotRespMaxReq = node.Attributes["botrespmaxreq"] != null ? node.Attributes["botrespmaxreq"].Value : Settings.BotRespMaxReq;
                        config.BotRespModSkip = node.Attributes["botrespmodskip"] != null ? node.Attributes["botrespmodskip"].Value : Settings.BotRespModSkip;
                        config.BotRespNoSong = node.Attributes["botrespnosong"] != null ? node.Attributes["botrespnosong"].Value : Settings.BotRespNoSong;
                        config.BotRespSuccess = node.Attributes["botrespsuccess"] != null ? node.Attributes["botrespsuccess"].Value : Settings.BotRespSuccess;
                        config.BotRespVoteSkip = node.Attributes["botrespvoteskip"] != null ? node.Attributes["botrespvoteskip"].Value : Settings.BotRespVoteSkip;
                        config.ClientId = node.Attributes["clientid"] != null ? node.Attributes["clientid"].Value : Settings.ClientId;
                        config.ClientSecret = node.Attributes["clientsecret"] != null ? node.Attributes["clientsecret"].Value : Settings.ClientSecret;
                        config.Color = node.Attributes["color"] != null ? node.Attributes["color"].Value : Settings.Color;
                        config.CustomPauseText = node.Attributes["customPauseText"] != null ? node.Attributes["customPauseText"].Value : Settings.CustomPauseText;
                        config.CustomPauseTextEnabled = ToBoolean(node.Attributes["customPause"] != null ? node.Attributes["customPause"].Value : Settings.CustomPauseTextEnabled.ToString());
                        config.Directory = node.Attributes["directory"] != null ? node.Attributes["directory"].Value : Settings.Directory;
                        config.DownloadCover = ToBoolean(node.Attributes["downloadcover"] != null ? node.Attributes["downloadcover"].Value : Settings.DownloadCover.ToString());
                        config.Language = node.Attributes["lang"] != null ? node.Attributes["lang"].Value : Settings.Language;
                        config.MaxSongLength = ToInt32(node.Attributes["maxsonglength"] != null ? node.Attributes["maxsonglength"].Value : Settings.MaxSongLength.ToString(CultureInfo.CurrentCulture));
                        config.MsgLoggingEnabled = ToBoolean(node.Attributes["msglogging"] != null ? node.Attributes["msglogging"].Value : Settings.MsgLoggingEnabled.ToString());
                        config.OpenQueueOnStartup = ToBoolean(node.Attributes["openqueueonstartup"] != null ? node.Attributes["openqueueonstartup"].Value : Settings.OpenQueueOnStartup.ToString());
                        config.OutputString = node.Attributes["outputString"] != null ? node.Attributes["outputString"].Value : Settings.OutputString;
                        config.OutputString2 = node.Attributes["outputString2"] != null ? node.Attributes["outputString2"].Value : Settings.OutputString2;
                        config.PosX = ToInt32(node.Attributes["posx"] != null ? node.Attributes["posx"].Value : Settings.PosX.ToString(CultureInfo.CurrentCulture));
                        config.PosY = ToInt32(node.Attributes["posy"] != null ? node.Attributes["posy"].Value : Settings.PosY.ToString(CultureInfo.CurrentCulture));
                        config.RefreshToken = node.Attributes["refreshtoken"] != null ? node.Attributes["refreshtoken"].Value : Settings.SpotifyRefreshToken;
                        config.SaveHistory = ToBoolean(node.Attributes["savehistory"] != null ? node.Attributes["savehistory"].Value : Settings.SaveHistory.ToString());
                        config.SpaceCount = ToInt32(node.Attributes["Spacecount"] != null ? node.Attributes["Spacecount"].Value : Settings.SpaceCount.ToString(CultureInfo.CurrentCulture));
                        config.SplitOutput = ToBoolean(node.Attributes["splitoutput"] != null ? node.Attributes["splitoutput"].Value : Settings.SplitOutput.ToString());
                        config.SpotifyDeviceId = node.Attributes["spotifydeviceid"] != null ? node.Attributes["spotifydeviceid"].Value : Settings.SpotifyDeviceId;
                        config.Systray = ToBoolean(node.Attributes["systray"] != null ? node.Attributes["systray"].Value : Settings.Systray.ToString());
                        config.Telemetry = ToBoolean(node.Attributes["telemetry"] != null ? node.Attributes["telemetry"].Value : Settings.Telemetry.ToString());
                        config.Theme = node.Attributes["theme"] != null ? node.Attributes["theme"].Value : Settings.Theme;
                        config.TwAcc = node.Attributes["twacc"] != null ? node.Attributes["twacc"].Value : Settings.TwAcc;
                        config.TwAutoConnect = ToBoolean(node.Attributes["twautoconnect"] != null ? node.Attributes["twautoconnect"].Value : Settings.TwAutoConnect.ToString());
                        config.TwChannel = node.Attributes["twchannel"] != null ? node.Attributes["twchannel"].Value : Settings.TwChannel;
                        config.TwOAuth = node.Attributes["twoauth"] != null ? node.Attributes["twoauth"].Value : Settings.TwOAuth;
                        config.TwRewardId = node.Attributes["twrewardid"] != null ? node.Attributes["twrewardid"].Value : Settings.TwRewardId;
                        config.TwSrCommand = ToBoolean(node.Attributes["twsrcommand"] != null ? node.Attributes["twsrcommand"].Value : Settings.TwSrCommand.ToString());
                        config.TwSrCooldown = twsrcooldown;
                        config.TwSrMaxReq = twsrmaxreq;
                        config.TwSrReward = ToBoolean(node.Attributes["twsrreward"] != null ? node.Attributes["twsrreward"].Value : Settings.TwSrReward.ToString());
                        config.TwSrUserLevel = ToInt32(node.Attributes["twsruserlevel"] != null ? node.Attributes["twsruserlevel"].Value : Settings.TwSrUserLevel.ToString(CultureInfo.CurrentCulture));
                        config.Upload = ToBoolean(node.Attributes["uploadSonginfo"] != null ? node.Attributes["uploadSonginfo"].Value : Settings.Upload.ToString());
                        config.UploadHistory = ToBoolean(node.Attributes["uploadhistory"] != null ? node.Attributes["uploadhistory"].Value : Settings.UploadHistory.ToString());
                        config.UseOwnApp = ToBoolean(node.Attributes["useownapp"] != null ? node.Attributes["useownapp"].Value : Settings.UseOwnApp.ToString());
                        config.UserBlacklist = node.Attributes["userblacklist"] != null ? node.Attributes["userblacklist"].Value : Settings.UserBlacklist;
                        config.Uuid = node.Attributes["uuid"] != null ? node.Attributes["uuid"].Value : Settings.Uuid;
                        config.TwSrMaxReqEveryone = ToInt32(node.Attributes["twsrmaxreqeveryone"] != null ? node.Attributes["twsrmaxreqeveryone"].Value : Settings.TwSrMaxReqEveryone.ToString(CultureInfo.CurrentCulture));
                        config.TwSrMaxReqVip = ToInt32(node.Attributes["twsrmaxreqvip"] != null ? node.Attributes["twsrmaxreqvip"].Value : Settings.TwSrMaxReqVip.ToString(CultureInfo.CurrentCulture));
                        config.TwSrMaxReqSubscriber = ToInt32(node.Attributes["twsrmaxreqsubscriber"] != null ? node.Attributes["twsrmaxreqsubscriber"].Value : Settings.TwSrMaxReqSubscriber.ToString(CultureInfo.CurrentCulture));
                        config.TwSrMaxReqVip = ToInt32(node.Attributes["twsrmaxreqvip"] != null ? node.Attributes["twsrmaxreqvip"].Value : Settings.TwSrMaxReqVip.ToString(CultureInfo.CurrentCulture));
                        config.TwSrMaxReqModerator = ToInt32(node.Attributes["twsrmaxreqmoderator"] != null ? node.Attributes["twsrmaxreqmoderator"].Value : Settings.TwSrMaxReqModerator.ToString(CultureInfo.CurrentCulture));
                        config.TwSrMaxReqBroadcaster = ToInt32(node.Attributes["twsrmaxreqbroadcaster"] != null ? node.Attributes["twsrmaxreqbroadcaster"].Value : Settings.TwSrMaxReqBroadcaster.ToString(CultureInfo.CurrentCulture));
                    }

                    ConvertConfig(config);
                }
            }
            catch (Exception ex)
            {
                Logger.LogExc(ex);
            }
        }

        public static void LoadConfig(string path = "")
        {
            if (path != "")
            {
                ReadXml(path);
            }
            else
            {
                // OpenfileDialog with settings initialdirectory is the path were the exe is located
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"
                };

                // Opening the dialog and when the user hits "OK" the following code gets executed
                if (openFileDialog.ShowDialog() == DialogResult.OK) ReadXml(openFileDialog.FileName);

                // This will iterate through all windows of the software, if the window is typeof 
                // Settingswindow (from there this class is called) it calls the method SetControls
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (Window window in Application.Current.Windows)
                        if (window.GetType() == typeof(Window_Settings))
                            ((Window_Settings)window).SetControls();
                });
            }
        }

        public static void ConvertConfig(Config cfg)
        {
            SpotifyCredentials sp = new SpotifyCredentials
            {
                AccessToken = cfg.AccessToken,
                RefreshToken = cfg.RefreshToken,
                DeviceId = cfg.SpotifyDeviceId,
                ClientId = cfg.ClientId,
                ClientSecret = cfg.ClientSecret,
            };
            WriteConfig(ConfigTypes.SpotifyCredentials, sp);

            TwitchCredentials twitchCredentials = new TwitchCredentials
            {
                AccessToken = "",
                ChannelName = cfg.TwChannel,
                ChannelId = "",
                BotAccountName = cfg.TwAcc,
                BotOAuthToken = cfg.TwOAuth
            };
            WriteConfig(ConfigTypes.TwitchCredentials, twitchCredentials);

            BotConfig botConfig = new BotConfig
            {
                BotCmdNext = cfg.BotCmdNext,
                BotCmdPos = cfg.BotCmdPos,
                BotCmdSkip = cfg.BotCmdSkip,
                BotCmdSkipVote = cfg.BotCmdSkipVote,
                BotCmdSong = cfg.BotCmdSong,
                BotCmdSkipVoteCount = cfg.BotCmdSkipVoteCount,
                BotRespBlacklist = cfg.BotRespBlacklist,
                BotRespError = cfg.BotRespError,
                BotRespIsInQueue = cfg.BotRespIsInQueue,
                BotRespLength = cfg.BotRespLength,
                BotRespMaxReq = cfg.BotRespMaxReq,
                BotRespModSkip = cfg.BotRespModSkip,
                BotRespNoSong = cfg.BotRespNoSong,
                BotRespSuccess = cfg.BotRespSuccess,
                BotRespVoteSkip = cfg.BotRespVoteSkip,
            };
            WriteConfig(ConfigTypes.BotConfig, botConfig);

            AppConfig appConfig = new AppConfig
            {
                AnnounceInChat = cfg.AnnounceInChat,
                AppendSpaces = cfg.AppendSpaces,
                AutoClearQueue = cfg.AutoClearQueue,
                Autostart = cfg.Autostart,
                CustomPauseTextEnabled = cfg.CustomPauseTextEnabled,
                DownloadCover = cfg.DownloadCover,
                MsgLoggingEnabled = cfg.MsgLoggingEnabled,
                OpenQueueOnStartup = cfg.OpenQueueOnStartup,
                SaveHistory = cfg.SaveHistory,
                SplitOutput = cfg.SplitOutput,
                Systray = cfg.Systray,
                Telemetry = cfg.Telemetry,
                TwAutoConnect = cfg.TwAutoConnect,
                TwSrCommand = cfg.TwSrCommand,
                TwSrReward = cfg.TwSrReward,
                Upload = cfg.Upload,
                UploadHistory = cfg.UploadHistory,
                UseOwnApp = cfg.UseOwnApp,
                MaxSongLength = cfg.MaxSongLength,
                PosX = cfg.PosX,
                PosY = cfg.PosY,
                SpaceCount = cfg.SpaceCount,
                TwSrCooldown = cfg.TwSrCooldown,
                TwSrMaxReq = cfg.TwSrMaxReq,
                TwSrMaxReqBroadcaster = cfg.TwSrMaxReqBroadcaster,
                TwSrMaxReqEveryone = cfg.TwSrMaxReqEveryone,
                TwSrMaxReqModerator = cfg.TwSrMaxReqModerator,
                TwSrMaxReqSubscriber = cfg.TwSrMaxReqSubscriber,
                TwSrMaxReqVip = cfg.TwSrMaxReqVip,
                TwSrUserLevel = cfg.TwSrUserLevel,
                ArtistBlacklist = cfg.ArtistBlacklist,
                Color = cfg.Color,
                CustomPauseText = cfg.CustomPauseText,
                Directory = cfg.Directory,
                Language = cfg.Language,
                OutputString = cfg.OutputString,
                OutputString2 = cfg.OutputString2,
                Theme = cfg.Theme,
                UserBlacklist = cfg.UserBlacklist,
                Uuid = cfg.Uuid,
            };
            WriteConfig(ConfigTypes.AppConfig, appConfig);
        }

        public static void WriteAllConfig(Configuration config)
        {
            WriteConfig(ConfigTypes.AppConfig, config.AppConfig);
            WriteConfig(ConfigTypes.BotConfig, config.BotConfig);
            WriteConfig(ConfigTypes.SpotifyCredentials, config.SpotifyCredentials);
            WriteConfig(ConfigTypes.TwitchCredentials, config.TwitchCredentials);
        }
    }

    public class Configuration
    {
        public AppConfig AppConfig { get; set; }
        public SpotifyCredentials SpotifyCredentials { get; set; }
        public TwitchCredentials TwitchCredentials { get; set; }
        public BotConfig BotConfig { get; set; }
    }

    public class SpotifyCredentials
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class TwitchCredentials
    {
        public string AccessToken { get; set; }
        public string ChannelName { get; set; }
        public string ChannelId { get; set; }
        public string BotAccountName { get; set; }
        public string BotOAuthToken { get; set; }
        public User TwitchUser { get; set; }
    }

    public class BotConfig
    {
        public bool BotCmdNext { get; set; }
        public bool BotCmdPos { get; set; }
        public bool BotCmdSkip { get; set; }
        public bool BotCmdSkipVote { get; set; }
        public bool BotCmdSong { get; set; }
        public int BotCmdSkipVoteCount { get; set; }
        public string BotRespBlacklist { get; set; }
        public string BotRespError { get; set; }
        public string BotRespIsInQueue { get; set; }
        public string BotRespLength { get; set; }
        public string BotRespMaxReq { get; set; }
        public string BotRespModSkip { get; set; }
        public string BotRespNoSong { get; set; }
        public string BotRespSuccess { get; set; }
        public string BotRespVoteSkip { get; set; }
    }

    public class AppConfig
    {
        public bool AnnounceInChat { get; set; }
        public bool AppendSpaces { get; set; }
        public bool AutoClearQueue { get; set; }
        public bool Autostart { get; set; }
        public bool CustomPauseTextEnabled { get; set; }
        public bool DownloadCover { get; set; }
        public bool MsgLoggingEnabled { get; set; }
        public bool OpenQueueOnStartup { get; set; }
        public bool SaveHistory { get; set; }
        public bool SplitOutput { get; set; }
        public bool Systray { get; set; }
        public bool Telemetry { get; set; }
        public bool TwAutoConnect { get; set; }
        public bool TwSrCommand { get; set; }
        public bool TwSrReward { get; set; }
        public bool Upload { get; set; }
        public bool UploadHistory { get; set; }
        public bool UseOwnApp { get; set; }
        public int MaxSongLength { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int SpaceCount { get; set; }
        public int TwSrCooldown { get; set; }
        public int TwSrMaxReq { get; set; }
        public int TwSrMaxReqBroadcaster { get; set; }
        public int TwSrMaxReqEveryone { get; set; }
        public int TwSrMaxReqModerator { get; set; }
        public int TwSrMaxReqSubscriber { get; set; }
        public int TwSrMaxReqVip { get; set; }
        public int TwSrUserLevel { get; set; }
        public string TwRewardId { get; set; }
        public int[] RefundConditons { get; set; }

        public string ArtistBlacklist { get; set; }
        public string Color { get; set; }
        public string CustomPauseText { get; set; }
        public string Directory { get; set; }
        public string Language { get; set; }
        public string OutputString { get; set; }
        public string OutputString2 { get; set; }
        public string Theme { get; set; }
        public string UserBlacklist { get; set; }
        public string Uuid { get; set; }
    }

    public class Config
    {
        // Create fields for each setting in the config file
        public bool AnnounceInChat { get; set; }
        public bool AppendSpaces { get; set; }
        public bool AutoClearQueue { get; set; }
        public bool Autostart { get; set; }
        public bool BotCmdNext { get; set; }
        public bool BotCmdPos { get; set; }
        public bool BotCmdSkip { get; set; }
        public bool BotCmdSkipVote { get; set; }
        public bool BotCmdSong { get; set; }
        public bool CustomPauseTextEnabled { get; set; }
        public bool DownloadCover { get; set; }
        public bool MsgLoggingEnabled { get; set; }
        public bool OpenQueueOnStartup { get; set; }
        public bool SaveHistory { get; set; }
        public bool SplitOutput { get; set; }
        public bool Systray { get; set; }
        public bool Telemetry { get; set; }
        public bool TwAutoConnect { get; set; }
        public bool TwSrCommand { get; set; }
        public bool TwSrReward { get; set; }

        public bool Upload { get; set; }
        public bool UploadHistory { get; set; }
        public bool UseOwnApp { get; set; }
        public int BotCmdSkipVoteCount { get; set; }
        public int MaxSongLength { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int SpaceCount { get; set; }
        public int TwSrCooldown { get; set; }
        public int TwSrMaxReq { get; set; }
        public int TwSrMaxReqBroadcaster { get; set; }
        public int TwSrMaxReqEveryone { get; set; }
        public int TwSrMaxReqModerator { get; set; }
        public int TwSrMaxReqSubscriber { get; set; }
        public int TwSrMaxReqVip { get; set; }
        public int TwSrUserLevel { get; set; }
        public string AccessToken { get; set; }
        public string ArtistBlacklist { get; set; }

        public string BotRespBlacklist { get; set; }
        public string BotRespError { get; set; }
        public string BotRespIsInQueue { get; set; }
        public string BotRespLength { get; set; }
        public string BotRespMaxReq { get; set; }
        public string BotRespModSkip { get; set; }
        public string BotRespNoSong { get; set; }
        public string BotRespSuccess { get; set; }
        public string BotRespVoteSkip { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Color { get; set; }
        public string CustomPauseText { get; set; }
        public string Directory { get; set; }
        public string Language { get; set; }
        public string NbUser { get; set; }
        public string NbUserId { get; set; }
        public string OutputString { get; set; }
        public string OutputString2 { get; set; }
        public string RefreshToken { get; set; }
        public string SpotifyDeviceId { get; set; }
        public string Theme { get; set; }
        public string TwAcc { get; set; }
        public string TwChannel { get; set; }
        public string TwOAuth { get; set; }
        public string TwRewardId { get; set; }
        public string UserBlacklist { get; set; }
        public string Uuid { get; set; }
    }
}
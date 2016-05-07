using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace njuTV_win10
{
    //private static StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
    #region SettingSaver 定义
    public class SettingSaver
    {
        public virtual void DeleteRecordObject(string Name) { }
        public virtual void AlterRecordObject<T>(string Name, T value) { }
        public virtual bool GetRecordObject<T>(string Name, ref T value) { return false; }
        #region 衍生
        public void DeleteRecordString(string Name)
        {
            DeleteRecordObject(Name);
        }
        public void AlterRecordString(string Name, string value)
        {
            AlterRecordObject(Name, value);
        }
        public void GetRecordString(string Name, ref string value)
        {
            GetRecordObject(Name, ref value);
        }
        #endregion
    }
    public class SettingSaver_Local : SettingSaver
    {
        private static ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
        #region Object-Local settings
        public override void DeleteRecordObject(string Name)
        {
            Settings.Values.Remove(Name);
        }
        public override void AlterRecordObject<T>(string Name, T value)
        {
            Settings.Values[Name] = value;
        }
        public override bool GetRecordObject<T>(string Name, ref T value)
        {
            if (Settings.Values.ContainsKey(Name))
            {
                value = (T)Settings.Values[Name];
                return true;
            }
            else
                return false;
        }
        #endregion
    }
    public class SettingSaver_Roam : SettingSaver
    {

        private static ApplicationDataContainer Settings = ApplicationData.Current.RoamingSettings;
        #region Object-Roaming settings
        public override void DeleteRecordObject(string Name)
        {
            Settings.Values.Remove(Name);
        }
        public override void AlterRecordObject<T>(string Name, T value)
        {
            Settings.Values[Name] = value;
        }
        public override bool GetRecordObject<T>(string Name, ref T value)
        {
            if (Settings.Values.ContainsKey(Name))
            {
                value = (T)Settings.Values[Name];
                return true;
            }
            else
                return false;
        }

        #endregion
    }
    #endregion
    public class NameManager
    {
        public const string ThemeSettingString = "Theme";
        public const string AutoPlaySettingString = "AutoPlay";
        public const string PreviewSettingString = "Preview";

    }
    #region Setting定义
    public enum SaveMethodEnum { None, Local, Roaming };
    public abstract class Setting
    {
        protected SettingSaver Saver;
        public Setting()
        {
        }
        public void SetSaver(SaveMethodEnum SaveMethod)
        {
            switch (SaveMethod)
            {
                case SaveMethodEnum.Local:
                    Saver = new SettingSaver_Local();
                    break;
                case SaveMethodEnum.Roaming:
                    Saver = new SettingSaver_Roam();
                    break;
                case SaveMethodEnum.None:
                    Saver = new SettingSaver();
                    break;
                default:
                    Saver = new SettingSaver_Local();
                    break;
            }
        }
        public abstract void LoadSetting();
        public abstract void SaveSetting();
        public abstract void ApplySetting();
        public abstract void ApplySetting(object OutSetting);
    }
    public abstract class BoolSetting_Local : Setting
    {
        string ID { get; set; }
        public bool State { get; set; }
        public BoolSetting_Local()
        {
            State = false;
            SetSaver(SaveMethodEnum.Local);
        }
        public override void LoadSetting()
        {
            bool State_sender = State;
            Saver.GetRecordObject(ID, ref State_sender);
            State = State_sender;
            ApplySetting();
        }
        public override void SaveSetting()
        {
            Saver.AlterRecordObject(ID, State);
        }
        public override void ApplySetting(object OutSetting)
        {
            State = (bool)OutSetting;
            SaveSetting();
            ApplySetting();
            Debug.WriteLine("修改了某个设置的值为:" + OutSetting.ToString());
        }

        public void SetID(string _ID)
        {
            ID = _ID;
        }
    }
    public class AutoPlaySetting : BoolSetting_Local
    {
        public AutoPlaySetting()
        {
            base.SetID("AutoPlay");
            LoadSetting();
        }
        public override void ApplySetting()
        {
            // do nothing here. just save something.
        }
    }
    #endregion
}

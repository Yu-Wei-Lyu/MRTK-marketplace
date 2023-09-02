namespace Assets.Scripts
{
    public class DialogDisplayOptions
    {
        public bool ShowLoadingIcon
        {
            get; set;
        }
        public bool ShowCounterZone
        {
            get; set;
        }
        public bool ShowConfirmButton
        {
            get; set;
        }
        public bool ShowCancelButton
        {
            get; set;
        }

        public static DialogDisplayOptions Default => new DialogDisplayOptions
        {
            ShowLoadingIcon = false,
            ShowCounterZone = false,
            ShowConfirmButton = false,
            ShowCancelButton = false
        };

        public static DialogDisplayOptions Loading => new DialogDisplayOptions
        {
            ShowLoadingIcon = true,
            ShowCounterZone = false,
            ShowConfirmButton = false,
            ShowCancelButton = false
        };

        public static DialogDisplayOptions Confirm => new DialogDisplayOptions
        {
            ShowLoadingIcon = false,
            ShowCounterZone = false,
            ShowConfirmButton = true,
            ShowCancelButton = false
        };

        public static DialogDisplayOptions ConfirmOrCancel => new DialogDisplayOptions
        {
            ShowLoadingIcon = false,
            ShowCounterZone = false,
            ShowConfirmButton = true,
            ShowCancelButton = true
        };

        public static DialogDisplayOptions CounterReturnOrCancel => new DialogDisplayOptions
        {
            ShowLoadingIcon = false,
            ShowCounterZone = true,
            ShowConfirmButton = true,
            ShowCancelButton = true
        };
    }
}
namespace RPG.Saving
{
    interface ISaveable
    {
        //---------------------------Everything function and member of a interface is public-------------------------------
        object CaptureState();
        void RestoreState(object state);
    }
}

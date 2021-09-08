namespace AI
{
    public interface ISubscriber
    {
        void OnStatUpdate(ObservableStat stat, StatType dataType);
    }

}

using Profile;
using Tools;

public class PlayerProfile
{
    public SubscriptionProperty<GameState> CurrentState { get; }
    public InputMethod InputMethod { get; private set; }

    public Car CurrentCar { get; }

    public PlayerProfile(float carSpeed)
    {
        CurrentState = new SubscriptionProperty<GameState>();
        CurrentCar = new Car(carSpeed);
    }

    public void SetInputMethod(int value)
    {
        InputMethod = (InputMethod)value;
    }
}


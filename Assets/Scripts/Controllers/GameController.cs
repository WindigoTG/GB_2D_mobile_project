using Tools;

public class GameController : BaseController
{
    public GameController(PlayerProfile playerProfile)
    {
        var leftMoveDiff = new SubscriptionProperty<float>();
        var rightMoveDiff = new SubscriptionProperty<float>();
        
        var tapeBackgroundController = new TapeBackgroundController(leftMoveDiff, rightMoveDiff);
        AddController(tapeBackgroundController);
        
        var inputGameController = new InputGameController(leftMoveDiff, rightMoveDiff, playerProfile.CurrentCar, playerProfile.InputMethod);
        AddController(inputGameController);
            
        var carController = new CarController(leftMoveDiff, rightMoveDiff);
        AddController(carController);
    }
}


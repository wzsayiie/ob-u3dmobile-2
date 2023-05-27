#import "UnityAppController.h"

@interface MainAppController : UnityAppController
@end

IMPL_APP_CONTROLLER_SUBCLASS(MainAppController)

@implementation MainAppController

- (BOOL)  /* .... */  application:(UIApplication *)application
    didFinishLaunchingWithOptions:(NSDictionary<UIApplicationLaunchOptionsKey, id> *)options
{
    [super application:application didFinishLaunchingWithOptions:options];

    return YES;
}

@end

//
//  MyUnityPluginUnityProtocol.m
//  MyUnityPluginBridge
//
//  Created by Andy.Kim on 2020/05/12.
//  Copyright © 2020 example. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MyUnityPluginUnityProtocol.h"
//#import "UnityInterface.h"

@implementation MyUnityPluginUnityProtocol

- (void)OnLoad {
    UnitySendMessage("MyUnityPlugin", "__fromnative_OnLoad", "");
}

- (void)OnCallTestFunc1WithStr:(NSString * _Nonnull)str {
    UnitySendMessage("MyUnityPlugin", "__fromnative_OnCallTestFunc1", [str UTF8String]);
}

- (void)OnCallTestFunc2WithNum:(NSString * _Nonnull)num {
    UnitySendMessage("MyUnityPlugin", "__fromnative_OnCallTestFunc2", [num UTF8String]);
}

@end

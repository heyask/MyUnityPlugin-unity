//
//  MyUnityPluginWrapper.m
//  MyUnityPluginBridge
//
//  Created by Andy.Kim on 2020/05/12.
//  Copyright © 2020 example. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MyUnityPlugin/MyUnityPlugin.h>
#import "MyUnityPluginWrapper.h"
#import "MyUnityPluginUnityProtocol.h"

#pragma mark - String Helpers


static NSString * const NSStringFromCString(const char *string)
{
    if (string != NULL) {
        return [NSString stringWithUTF8String:string];
    } else {
        return nil;
    }
}

static const char * const CStringFromNSString(NSString *string)
{
    if (string != NULL) {
        return [string cStringUsingEncoding:NSUTF8StringEncoding];
    } else {
        return nil;
    }
}

#pragma mark - C interface

extern "C" {
    
    void __IOS_Initialize()
    {
        MyUnityPluginUnityProtocol *callback = [[MyUnityPluginUnityProtocol alloc] init];
        [[MyUnityPluginContoller GetInstance] InitializeWithPluginDelegate:callback];
    }
    
    const char* __IOS_TestFunc1(const char* _str) {
        NSString *_strNS = NSStringFromCString(_str);
        return strdup(CStringFromNSString([[MyUnityPluginContoller GetInstance] TestFunc1WithStr:_strNS]));
    }
    
    NSInteger __IOS_TestFunc2(int _num) {
        return [[MyUnityPluginContoller GetInstance] TestFunc2WithNum:_num];
    }
}

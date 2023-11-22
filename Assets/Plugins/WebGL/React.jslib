mergeInto(LibraryManager.library, {
  CoppyAddress: function(str) {
    // var fn = typeof UTF8ToString === 'function' ? UTF8ToString : Pointer_stringify;
    // navigator.clipboard.writeText(fn(str));
    window.dispatchReactUnityEvent("CoppyAddress",str);
  },

  //get Account
  GetAccount: function () {
    window.dispatchReactUnityEvent("GetAccount");
  },
  //buy Turn
  BuyTurn: function (count, type) {
    window.dispatchReactUnityEvent("BuyTurn",count, type);
  },
  Approve: function () {
    window.dispatchReactUnityEvent("Approve");
  },
  QuitGame: function () {
    window.dispatchReactUnityEvent("QuitGame");
  },
  MintNft: function (rarity,nonce,signature,id) {
   
    window.dispatchReactUnityEvent("MintNft",rarity,nonce,UTF8ToString(signature),UTF8ToString(id));
  },
  
  
   // override default callback
   emscripten_set_wheel_callback_on_thread: function (
    target,
    userData,
    useCapture,
    callbackfunc,
    targetThread
  ) {
    target = findEventTarget(target);
 
    // the fix
    if (!target) {
      return -4;
    }
 
    if (typeof target.onwheel !== 'undefined') {
      registerWheelEventCallback(
        target,
        userData,
        useCapture,
        callbackfunc,
        9,
        'wheel',
        targetThread
      );
      return 0;
    } else {
      return -1;
    }
  }

});


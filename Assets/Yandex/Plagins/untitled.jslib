mergeInto(LibraryManager.library, {
    ShowAds: function () {
      ysdk.adv.showFullscreenAdv({
          callbacks: {
              onClose: function(wasShown) {
              },
              onError: function(error) {
              }
          }
      })
    },

});

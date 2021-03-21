# Unity Common

[![Test](https://github.com/litefeel/UnityCommon/workflows/Test/badge.svg)](https://github.com/litefeel/UnityCommon/actions)
[![](https://img.shields.io/github/release/litefeel/UnityCommon.svg?label=latest%20version)](https://github.com/litefeel/UnityCommon/releases)
[![](https://img.shields.io/github/license/litefeel/UnityCommon.svg)](https://github.com/litefeel/UnityCommon/blob/main/LICENSE.md)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/litefeel)

[UnityCommon](https://github.com/litefeel/UnityCommon) unity common

## Install

#### Using npm (Ease upgrade in Package Manager UI)**Recommend**

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
``` js
{
  {
  "scopedRegistries": [
    {
      "name": "My Registry",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.litefeel"
      ]
    }
  ],
  "dependencies": {
    "com.litefeel.unitycommon": "0.0.3",
    ...
  }
}
```

#### Using git

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
``` js
{
  "dependencies": {
    "com.litefeel.unitycommon": "https://github.com/litefeel/UnityCommon.git#0.0.3",
    ...
  }
}
```

#### Using .zip file (for Unity 5.0+)

1. Download `Source code` from [Releases](https://github.com/litefeel/UnityCommon/releases)
2. Extract the package into your Unity project



## Support

- Create issues by [issues][issues] page
- Send email to me: <litefeel@gmail.com>

[issues]: https://github.com/litefeel/UnityCommon/issues 

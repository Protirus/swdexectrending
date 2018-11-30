# {CWoC} Software Delivery Execution Trending

![](https://img.shields.io/badge/language-c%23-green.svg)
![](https://img.shields.io/badge/tag-smp-yellow.svg)
![](https://img.shields.io/badge/tag-symantec-yellow.svg)
![](https://img.shields.io/badge/tag-softwaredelivery-yellow.svg)

Software Delivery Execution Trending for ![SMP](docs/images/smp.png) Symantec Management Platform

---

See the example site: [swdexectrending](https://protirus.github.io/swdexectrending)

---

There are also some **Docs**

- [Guide](/docs/GUIDE.md)
- [ChangeLog](/docs/CHANGELOG.md)
- [Build](/docs/BUILD.md)
- [Switches](/docs/SWITCHES.md)

--

## Setup

Depending on which drive you installed your ![SMP](docs/images/smp.png) SMP on will change the following, but assuming it is the default:

You will want to create a new folder `swdtrending` and copy the exe to that folder.

Then setup a new Task in the SMP to run the following

```
cd "c:\program files\altiris\notification sever\web\swdtrending"
swdtrending > trending_data.js
```

```
cd "c:\program files\altiris\notification sever\web\swdtrending"
swdtrending /msft > msft_trending_data.js`
```

The [index.html](/docs/index.html) or the [mstf.html](/docs/mstf.html) files both have a `script` tag in the `<head>` that you would need to modify it depending on the name of the file you output above:

```html
<script type="text/javascript" src="swd_trending_data.js"></script>
```

```html
<script type="text/javascript" src="msft_trending_data.js"></script>
```

Hosting the site: 

`c:\program files\altiris\notification sever\web\swdtrending`
- `http://localhost/altiris/swdtrending/`

Or

`C:\inetpub\wwwwroot\SWDTrending`
- `http://localhost/swdtrending/`

## Switches

See the [Switches](#Switches) page for more info:

- `SWDExecTrending /raw`
- `SWDExecTrending /msft`

- `SWDExecTrending --version` or `SWDExecTrending /version`

---

Original [swdexectrending](https://github.com/somewhatsomewhere/swdexectrending) from [Ludovic Ferre
](https://www.symantec.com/connect/user/ludovic-ferre)

>[END OF "SUPPORT" NOTICE]

>Hello everyone, after close to 5 years maintaining various tools around Symantec Connect this legacy is turning to be more of a burden than anything else.
>It's still is a great set of tool and they all have their use, but as such I'm not going to maintain them anymore.
>The source code for this tool may still change over time, and can be found on Github: https://github.com/somewhatsomewhere?tab=repositories

>[/END OF "SUPPORT" NOTICE]

---

Check the [WIKI](https://github.com/Protirus/swdexectrending/wiki) or the following Symantec Connect Article(s)

- [{CWoC} Software Delivery Execution Trending](https://www.symantec.com/connect/downloads/cwoc-software-delivery-execution-trending)
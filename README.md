# PaniniStickerWebAPI
[![license](https://img.shields.io/github/license/aldovilardy/PaniniStickerWebAPI.svg)](https://github.com/aldovilardy/PaniniStickerWebAPI/blob/master/LICENSE)

<a href="https://www.mashape.com/aldovilardy/panini-sticker-generator?&amp;utm_campaign=mashape5-embed&amp;utm_medium=button&amp;utm_source=panini-sticker-generator&amp;utm_content=anchorlink&amp;utm_term=text-light"><img src="https://d1g84eaw0qjo7s.cloudfront.net/images/badges/badge-text-light-e9e35ecb.png" alt="Mashape" width="200" height="38"></a>

![PaniniStickerWebAPILogo](/PaniniStickerWebAPI/Content/images/Logos/AppLogo.png)

Service to make your own Fifa Russia 2018 panini stickers.

## Using API REST
  
| Basic Concepts |  |
| --- | --- |
| URL | http://paninistickerwebapi.azurewebsites.net/api/ |
| Request | *POST* HTTP format JSON (application/json) |
| Responses | *JSON format* HTTP 200 code JSON (application/json) |

| Request Sticker Generator |  |
| --- | --- |
| URL | http://paninistickerwebapi.azurewebsites.net/api/StickerGenerator |
| Method | POST |

### Request JSON format

| JSON Key | Example Value | Description | 
| --- | --- | --- |
| photoUrl | "http://i.somethingawful.com/inserts/articlepics/photoshop/08-06-04-terminators/CapnDeviance.jpg" | The url of the source image |
| frame | "NON" | The Country Futbol Team using uppercase like Colombia "COL" or Russia "RUS" if you don't want a team you can select None "NON"  |
| fullName | "John Doe" | Name and Lastname of the person or character in the photo |
| position | "None" | You can select the player position like "Goalkeeper", "Defender", "Midfilder", "Forward" or "None"  |
| DoB | "0001-01-01T00:00:00" | The date/time of birthday in date time fortmat yyyy-MM-ddThh:mm:ss  |
| club | "Example Futbol Club" | The club, home town or nickname that you like to write  |
| country | "NON" | The Country using uppercase like Colombia "COL" or Russia "RUS" if you don't want a team you can select None "NON"  |
| debut | 1986 | The year of debut player |

**Here a JSON request example:**
```json
{
  "photoUrl": "http://i.somethingawful.com/inserts/articlepics/photoshop/08-06-04-terminators/CapnDeviance.jpg",
  "frame": "NON",
  "fullName": "John Doe",
  "position": "None",
  "DoB": "1985-04-10T00:00:00",
  "club": "Example Futbol Club",
  "country": "NON",
  "debut": 1986
}
```
**And Here a JSON response example:**
```json
{
    "status_code": 200,
    "success": {
        "message": "image uploaded",
        "code": 200
    },
    "image": {
        "title": "170620180133225695622 Sticker",
        "name": "170620180133225695622_Sticker",
        "extension": "png",
        "original_filename": "170620180133225695622_Sticker.png",
        "width": "601",
        "height": "782",
        "size": 669201,
        "thumb_size": "66030",
        "medium_size": "0",
        "time": "1529199203",
        "expiration": "0",
        "description": null,
        "original_exifdata": null,
        "likes": "0",
        "hash": null,
        "views_html": "0",
        "views_hotlink": "0",
        "access_html": "0",
        "access_hotlink": "0",
        "file": {
            "resource": {
                "chain": {
                    "image": "https://image.ibb.co/huRw7y/170620180133225695622_Sticker.png",
                    "thumb": "https://thumb.ibb.co/jUkCfJ/170620180133225695622_Sticker.png"
                },
                "chain_code": {
                    "image": "huRw7y",
                    "thumb": "jUkCfJ"
                }
            }
        },
        "is_animated": 0,
        "nsfw": 0,
        "id_encoded": "jUkCfJ",
        "filename": "170620180133225695622_Sticker.png",
        "mime": "image/png",
        "url": "https://image.ibb.co/huRw7y/170620180133225695622_Sticker.png",
        "ratio": 0.768542199488491,
        "size_formatted": "669.2 KB",
        "url_viewer": "https://ibb.co/jUkCfJ",
        "url_viewer_preview": "https://ibb.co/jUkCfJ",
        "url_viewer_thumb": "https://ibb.co/jUkCfJ",
        "image": {
            "filename": "170620180133225695622_Sticker.png",
            "name": "170620180133225695622_Sticker",
            "mime": "image/png",
            "extension": "png",
            "url": "https://image.ibb.co/huRw7y/170620180133225695622_Sticker.png",
            "size": 669201
        },
        "thumb": {
            "filename": "170620180133225695622_Sticker.png",
            "name": "170620180133225695622_Sticker",
            "mime": "image/png",
            "extension": "png",
            "url": "https://thumb.ibb.co/jUkCfJ/170620180133225695622_Sticker.png",
            "size": "66030"
        },
        "display_url": "https://thumb.ibb.co/jUkCfJ/170620180133225695622_Sticker.png",
        "display_width": "180",
        "display_height": "180",
        "views_label": "views",
        "likes_label": "likes",
        "how_long_ago": "moments ago",
        "date_fixed_peer": "2018-06-17 01:33:23",
        "title_truncated": "170620180133225695622 Sti...",
        "title_truncated_html": "170620180133225695622 Sti...",
        "is_use_loader": false
    },
    "request": {
        "type": "file",
        "action": "upload",
        "privacy": "undefined",
        "timestamp": "1527541040686",
        "auth_token": "c08e59c27aeeb1cb0f126e3ed3856cf1fd675d06",
        "nsfw": "0"
    },
    "status_txt": "OK"
}
```
**Image result:**

*Before*

![Before](https://i.somethingawful.com/inserts/articlepics/photoshop/08-06-04-terminators/CapnDeviance.jpg)


*After*

![After](https://i.ibb.co/vPcHhnw/170620180133225695622-Sticker.png)


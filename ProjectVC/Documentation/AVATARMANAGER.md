<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a name="readme-top"></a>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->


# Avatar Manager

The Avatar Manager is a core script which handles the loading of Ready Player Me avatars at runtime.

It handles predowloaded preview avatars as well as online created custom avatars.

## Predownloaded Avatars
Have a look at [Default Avatars](DEFAULTAVATARS.md)

## Custom Avatars
When the player interacts with the Ready Player Me Webview, an avatar link gets generated and parsed to the Avatar Manager.
Then the AvatarManager handles the download of the RPM avatar with the provided URL. 
When the AvatarManager downloaded the avatar successfully it throws the OnAvatarDownloaded Event and stores the object.



<hr>


[Back to Readme](README.md)


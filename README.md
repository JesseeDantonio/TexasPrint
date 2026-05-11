# Prérequis Développement

- [Dotnet](https://dotnet.microsoft.com/fr-fr/download)
- [SumatraPDF](https://www.sumatrapdfreader.org/download-free-pdf-viewer) (Version Portable)

## NuGet Packages

Commande à executer avec les NuGet

```console
dotnet add package
```

- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Configuration.Binder
- Microsoft.Extensions.Hosting;

Pack Extension VSCode à installer

- [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

## Compilation

```console
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```


# Prérequis Utilisation

1. Télécharger la version portable de
[SumatraPDF](https://www.sumatrapdfreader.org/download-free-pdf-viewer) 

2. Décompresser le .zip, placer le .exe où vous voulez

3. Configurer le fichier appsetting.json

4. Lancer TexasPrint.exe




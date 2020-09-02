# OxyLauncher

<a href="https://forthebadge.com/"><img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg" alt="forthebadge" height="28"/></a>

> OxyLauncher est un petit outil pour lancer rapidement ses applications

## NÉCÉSSITE [Microsoft .NET Core 3.1](https://dotnet.microsoft.com/download)

---

# Installation

Télécharger l'archive, dézippez là, lancer `OxyLauncher.exe` ou via la commande line :

```powershell
dotnet OxyLauncher.dll
```

# Utilisation

- Clic gauche pour lancer une application
- Clic droit pour ouvrir son répertoire
- Ctrl + Clic droit pour l'ajouter au applications personnalisées

![screen](https://i.imgur.com/egOzVUi.png)

Après un premier lancement, OxyLauncher générera un fichier `settings.json` de cette forme :

```json
{
  "AppFolder": "C:\\Program Files",
  "Exceptions": [],
  "CustomApplications": [],
  "Editor": "C:\\WINDOWS\\system32\\notepad.exe"
}
```

Je vous conseille vivement de le modifier selon vos envies.

- `AppFolder` représente un chemin vers le répertoire où sont situés vos applications.
- `Exceptions` représente les noms des applications que vous ne voulez pas afficher (exemple : `"Exceptions": ["cmder","dotnet"]` cachera les applications ayant pour nom `cmder` ou `dotnet`).
- `CustomApplications` représente les paramètres personnalisées. Utile pour choisir un executable particulier, changer le nom ou rajouter des arguments.
  - `exe_path` représente le chemin **relatif** de l'exécutable **par rapport à `AppFolder`**. (Exemple : `"exe_path": "Teamviewer\\TeamViewer.exe"`)
- `Editor` représente le chemin **complet** vers l'éditeur pour ouvrir les paramètres.

## Auteurs

- [**OxyTom**](https://github.com/oxypomme) - [@OxyTom](https://twitter.com/OxyT0m8)

Lisez la liste des [contributeurs](https://github.com/oxypomme/OxyLauncher/contributors) qui ont participé à ce projet.

[![license](https://img.shields.io/github/license/oxypomme/OxyLauncher?style=for-the-badge)](https://github.com/oxypomme/OxyLauncher/blob/master/LICENSE)

# Stage_M2

# Guide d'exécution et de déploiement

Ce document décrit la procédure de lancement de la suite logicielle, la configuration des conteneurs WComp ainsi que les étapes de compilation et de déploiement de **AADesignerWeaverBeans**.

---

## 1. Exécution des logiciels et configuration WComp

### Étape 1 : Lancement des outils
Lancer simultanément les logiciels suivants :
* **WComp**
* **WizardDesigner**
* **AADesigner**

### Étape 2 : Création des conteneurs dans WComp
Dans l'interface de **WComp**, créer deux conteneurs :
1. `appli`
2. `weaver`

### Étape 3 : Import dans le conteneur Weaver
Copier dans le sous-dossier `Beans` du répertoire d'installation de **SharpDevelop** les beans présent dans :
```text
suite_logicielle/SharpDevelop3.2-src_release/Beans
```
Dans le conteneur **weaver**, importer le fichier :
```text
suite_logicielle/SharpDevelop3.2-src_release/SharpWCompContainer/weaver.wcc
```

### Étape 4 : Sélection du conteneur dans WizardDesigner
* Ouvrir **WizardDesigner** et sélectionner le conteneur nommé `appli`.

---

## 2. Modification et recompilation de AADesignerWeaverBeans

### Étape 1 : Ouverture et compilation
1. Ouvrir le projet `AADesignerWeaverBeans.csproj` dans votre IDE.
2. Effectuer les modifications nécessaires.
3. Recompiler le projet en mode **Release**.

### Étape 2 : Récupération des artefacts
Dans le dossier `bin/Release/`, récupérer les deux fichiers générés :
* `AADesignerWeaverBeans.dll`
* `AADesignerWeaverBeans.pdb`

### Étape 3 : Déploiement
Copier ces deux fichiers dans le sous-dossier `Beans` du répertoire d'installation de **SharpDevelop** :
```text
<dossier_installation_SharpDevelop>/Beans/
```

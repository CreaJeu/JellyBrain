# 🧠 JellyBrain
Salut ! ✌️  
Bienvenue sur le repo de JellyBrain, un petit jeu [Godot](https://godotengine.org/) Engine créé par notre petit groupe [Créajeu](https://www.ascreb.org/clubs-pages/creajeu/) 👾

---
# 🤝 Comment participer ?
Intéressé.e de contribuer au jeu ?  
On prend tout type de compétences : artiste, développeur, musicien, graphiste, couturier, maçon, etc... 😁  
Tout le monde peut proposer :
- Corrections de bugs
- Nouvelles fonctionnalités
- Optimisations
- Améliorations artistiques
- Expérimentations

Pour que ce ne soit pas trop le bazar, voici comment on fonctionne 👇

---
# Pré-requis
- Installer [`git`](https://git-scm.com/) 
- Installer [`git-lfs`](https://git-lfs.com/)  
Certains fichiers sont volumineux, on utilise donc `git-lfs`.  
Une fois installé, vous pouvez utiliser Git normalement.

# Nos outils
- Moteur de jeu : [Godot Engine](https://godotengine.org/)
- Langage principal : C#
- 3D : Blender

---
# Contribution EXTERNE (vous n’êtes pas contributeur "officiel")
Si vous ne faites pas partie de l’équipe Créajeu, vous devez passer par un **fork**.

## 🥇 Étape 1 : Forker le projet
Sur GitHub, cliquez sur **Fork**.
Cela crée votre propre copie du projet :
`github.com/VotreNom/JellyBrain`

## 🥈 Étape 2 : Cloner votre fork
```sh
git clone https://github.com/VotreNom/JellyBrain.git cd JellyBrain
```
## 🥉 Étape 3 : Ajouter le repo officiel comme "upstream"
```sh
git remote add upstream https://github.com/CreaJeu/JellyBrain.git
```
Cela permet de récupérer les mises à jour officielles.

## 🔄 Étape 4 : Se placer sur `dev`
```sh
git checkout dev git pull upstream dev
```

## 🌿 Étape 5 : Créer votre branche
Nous utilisons ces mots-clés :
- **feature** : nouvelle fonctionnalité
- **bugfix** : correction de bug
- **hotfix** : bug critique
- **chore** : nettoyage / optimisation
- **experiment** : expérimentation

Exemple :
```sh
git checkout -b feature/dash 
```
 
Format général :
```sh
git checkout -b <mot-clé>/<nom-modification>
```
## 🚀 Étape 6 : Pusher sur votre fork
```sh
git push origin feature/dash
```

## 🔁 Étape 7 : Ouvrir une Pull Request
Sur GitHub :  
`VotreFork/feature/dash → CreaJeu/JellyBrain:dev`  
Nous relirons votre proposition avant intégration ✨

___
# 🏠 Contribution INTERNE (membres officiels Créajeu)
Les membres officiels ayant accès en écriture au repo principal peuvent travailler plus directement.

Workflow recommandé :
## 1️⃣ Se placer sur `dev`
```sh
git checkout dev
git pull origin dev
```
## 2️⃣ Créer une branche
Même convention :
```sh
git checkout -b feature/dash
```
## 3️⃣ Pusher sur le repo principal
```sh
git push origin feature/dash
```

## 🔎 Pull Request (recommandé mais pas obligatoire)
Même si ce n’est pas strictement obligatoire pour les membres officiels,  
il est **fortement recommandé** de passer par une Pull Request pour :
- Avoir un second regard
- Éviter les conflits
- Maintenir une bonne qualité de code
- Documenter les changements 

---
# 🔒 Branches protégées
Les branches `main` et `dev` sont protégées :  
- Pas de push direct sur `main`
- Les modifications importantes passent par `dev`
- `main` correspond à une version stable
---
# 📌 Résumé

| Type de contributeur | Fork obligatoire | PR obligatoire |
| -------------------- | ---------------- | -------------- |
| Externe              | ✅ Oui            | ✅ Oui        |
| Membre officiel      | ❌ Non            | Recommandé    |

___
___

[LICENSE](./LICENSE)
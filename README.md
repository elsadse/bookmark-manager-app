
# Application de gestionnaire de favoris
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/elsadse/bookmark-manager-app/deployment.yaml)
![GitHub package.json version](https://img.shields.io/github/package-json/v/elsadse/bookmark-manager-app)
![GitHub repo size](https://img.shields.io/github/repo-size/elsadse/bookmark-manager-app)
![GitHub License](https://img.shields.io/github/license/elsadse/bookmark-manager-app)
![GitHub top language](https://img.shields.io/github/languages/top/elsadse/bookmark-manager-app)
![GitHub commit activity](https://img.shields.io/github/commit-activity/t/elsadse/bookmark-manager-app)
![GitHub Repo stars](https://img.shields.io/github/stars/elsadse/bookmark-manager-app)

Application de gestion de favoris responsive avec des fonctionnalités d'ajout, de modification, d'archivage, de recherche et de filtrage.

## Configuration

- **Configuration de l'environnement de devéloppement:**

```bash
git clone https://github.com/elsadse/bookmark-manager-app.git
cd bookmark-manager-app
bun install
bun run dev
```

- **Production build et déploiement:**

```bash
git clone https://github.com/elsadse/bookmark-manager-app.git
cd bookmark-manager-app
bun install
bun run build
bun run preview
```

## Caractéristiques

- Ajoutez de nouveaux favoris avec un titre, une description, l'URL du site web et des balises.
- Affichez tous vos favoris.
- Consultez les détails des favoris, notamment leur favicon, leur titre, leur URL, leur description, leurs balises, le nombre de vues, la date de la dernière visite et la date d'ajout.
- Recherchez des favoris par titre dans la barre de recherche.
- Filtrez les favoris en sélectionnant une ou plusieurs balises dans la barre latérale.
- Réinitialisez les filtres de balises pour afficher à nouveau tous les favoris
- Affichez les favoris archivés
- Archivez les favoris pour les supprimer de la vue principale sans les supprimer
- Épinglez/dépinglez des favoris pour garder les plus importants facilement accessibles
- Modifiez les favoris existants pour mettre à jour leurs détails
- Copiez les URL des favoris dans le presse-papiers.
- Visitez les sites web mis en signet directement depuis l'application.
- Triez les favoris par « Ajoutés récemment », « Visités récemment » ou « Les plus visités ».
- Basculez entre les thèmes de couleurs claires et foncées.
- Affichez la mise en page optimale pour l'interface en fonction de la taille de l'écran de votre appareil.
- Affichez les états de survol et de mise au point pour tous les éléments interactifs de la page.


## Stack technologique

- Typescript
- React 19 + Vite
- Tailwind CSS
- Githup Pages + Githup Action


## Auteurs

- [@elsadse](https://www.github.com/elsadse)
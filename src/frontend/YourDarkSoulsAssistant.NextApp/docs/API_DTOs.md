# API DTO Reference for Backend Developer

This document contains all Data Transfer Object (DTO) specifications that the frontend expects from the backend API. The backend should return JSON responses matching these structures.

---

## Table of Contents

1. [Authentication DTOs](#1-authentication-dtos)
2. [User DTOs](#2-user-dtos)
3. [Build/Equipment DTOs](#3-buildequipment-dtos)
4. [Wiki DTOs](#4-wiki-dtos)
5. [Video Guides DTOs](#5-video-guides-dtos)
6. [Gallery/Archive DTOs](#6-galleryarchive-dtos)
7. [API Endpoints Summary](#7-api-endpoints-summary)

---

## 1. Authentication DTOs

### AuthResponseDTO (Login/Register Response)
**Endpoint:** `POST /api/user/Auth/login`, `POST /api/user/Auth/register`

```json
{
  "isSuccess": true,
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_string",
  "errorMessage": null,
  "userName": "TarnishedOne",
  "role": "user"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| isSuccess | boolean | Yes | Whether the auth operation succeeded |
| accessToken | string | null | No | JWT access token (null if failed) |
| refreshToken | string | null | No | Refresh token for token renewal |
| errorMessage | string | null | No | Error message if isSuccess is false |
| userName | string | null | No | User's username |
| role | string | null | No | User role: "admin" or "user" |

### LoginRequestDTO
**Endpoint:** `POST /api/user/Auth/login`

```json
{
  "email": "user@example.com",
  "password": "securePassword123"
}
```

### RegisterRequestDTO
**Endpoint:** `POST /api/user/Auth/register`

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "userName": "TarnishedOne",
  "email": "user@example.com",
  "password": "securePassword123",
  "confirmPassword": "securePassword123"
}
```

### RefreshTokenRequestDTO
**Endpoint:** `POST /api/user/Auth/refresh-token`

```json
{
  "accessToken": "expired_access_token",
  "refreshToken": "valid_refresh_token"
}
```

---

## 2. User DTOs

### UserProfileDTO
**Endpoint:** `GET /api/user/Account/profile` (Requires JWT Authorization header)

```json
{
  "id": "uuid-string",
  "email": "user@example.com",
  "userName": "TarnishedOne",
  "firstName": "John",
  "lastName": "Doe",
  "role": "user",
  "avatarPath": "/avatars/user123.jpg",
  "joinDate": "2024-01-15T10:30:00Z",
  "isAdmin": false,
  "level": 125,
  "covenant": "Warriors of Sunlight",
  "buildsCount": 5
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | string | Yes | Unique user identifier (UUID) |
| email | string | Yes | User's email address |
| userName | string | Yes | Display username |
| firstName | string | Yes | User's first name |
| lastName | string | Yes | User's last name |
| role | string | Yes | "admin" or "user" |
| avatarPath | string | null | No | Path to user's avatar image |
| joinDate | string (ISO 8601) | Yes | Account creation date |
| isAdmin | boolean | Yes | Whether user has admin privileges |
| level | number | No | User's character level (default: 1) |
| covenant | string | null | No | User's chosen covenant/faction |
| buildsCount | number | No | Number of builds created |

### UpdateUserRequestDTO
**Endpoint:** `PUT /api/user/Account/profile`

```json
{
  "id": "uuid-string",
  "firstName": "John",
  "lastName": "Doe",
  "userName": "NewUsername",
  "email": "newemail@example.com",
  "covenant": "Blue Sentinels"
}
```

All fields are optional except `id`.

---

## 3. Build/Equipment DTOs

### ItemDataDTO
Used for weapons, armor, talismans, consumables, arrows, shields.

```json
{
  "id": "item-uuid",
  "name": "Moonveil",
  "type": "Katana",
  "category": "weapon",
  "attackType": "Slash/Pierce",
  "fpCost": "17",
  "weight": "6.5",
  "image": "/items/weapons/moonveil.png",
  "attack": {
    "physical": "73",
    "magic": "87",
    "fire": "0",
    "lightning": "0",
    "holy": "0",
    "critical": "100"
  },
  "guard": {
    "physical": "31",
    "magic": "57",
    "fire": "26",
    "lightning": "26",
    "holy": "26",
    "boost": "31"
  },
  "scaling": {
    "str": "E",
    "dex": "D",
    "int": "C",
    "fai": "-",
    "arc": "-"
  },
  "required": {
    "str": "12",
    "dex": "18",
    "int": "23",
    "fai": "0",
    "arc": "0"
  },
  "passiveEffects": ["Causes blood loss buildup (50)"]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | string | No | Item unique identifier |
| name | string | Yes | Item display name |
| type | string | Yes | Item subtype (e.g., "Katana", "Medium Helm") |
| category | string | No | "weapon", "armor", "talisman", "consumable", "arrow", "shield" |
| attackType | string | No | Attack damage type |
| fpCost | string | No | FP cost for weapon skill |
| weight | string | Yes | Item weight value |
| image | string | No | Path to item image |
| attack | object | Yes | Attack power values (all strings) |
| guard | object | Yes | Guard values (all strings) |
| scaling | object | Yes | Stat scaling grades (E, D, C, B, A, S, or "-") |
| required | object | Yes | Required stats to wield |
| passiveEffects | string[] | No | Array of passive effect descriptions |

### CharacterStatsDTO

```json
{
  "level": 150,
  "runesHeld": 500000,
  "vigor": 60,
  "mind": 30,
  "endurance": 40,
  "strength": 50,
  "dexterity": 40,
  "intelligence": 20,
  "faith": 25,
  "arcane": 15
}
```

### EquipmentSlotsDTO

```json
{
  "weapons": [ItemDataDTO, ItemDataDTO, null],
  "shields": [ItemDataDTO, null, null],
  "arrows": [ItemDataDTO, null, null, null],
  "armor": {
    "head": ItemDataDTO | null,
    "chest": ItemDataDTO | null,
    "hands": ItemDataDTO | null,
    "legs": ItemDataDTO | null
  },
  "talismans": [ItemDataDTO, ItemDataDTO, null, null],
  "consumables": [null, null, null, null, null, null, null, null, null, null]
}
```

### BuildDataDTO
**Endpoints:** 
- `GET /api/builds` - List all builds
- `GET /api/builds/{id}` - Get single build
- `POST /api/builds` - Create build
- `PUT /api/builds/{id}` - Update build

```json
{
  "id": "build-uuid",
  "name": "Moonveil Mage Build",
  "stats": CharacterStatsDTO,
  "equipment": EquipmentSlotsDTO,
  "createdAt": "2024-03-15T10:30:00Z",
  "updatedAt": "2024-03-16T14:20:00Z"
}
```

---

## 4. Wiki DTOs

### WikiItemDTO (List View)
**Endpoint:** `GET /api/wiki/items`

```json
{
  "id": "item-uuid",
  "name": "Moonveil",
  "category": "weapons",
  "subcategory": "Katana",
  "image": "/weapons/moonveil.png",
  "description": "A katana forged in the Shattering...",
  "stats": {
    "Physical": 73,
    "Magic": 87,
    "Weight": 6.5
  },
  "location": "Gael Tunnel",
  "difficulty": "Normal"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | string | Yes | Unique identifier |
| name | string | Yes | Item name |
| category | string | Yes | "weapons", "armor", "talismans", "bosses", "guides", "locations", "magic" |
| subcategory | string | No | Type within category (e.g., "Katana", "Heavy Armor") |
| image | string | No | Image path |
| description | string | Yes | Short description |
| stats | object | No | Key-value pairs of quick stats |
| location | string | No | Where to find it |
| difficulty | string | No | For bosses only: "Easy", "Normal", "Hard", "Extreme" |

### WikiItemDetailDTO (Detail View)
**Endpoint:** `GET /api/wiki/items/{category}/{id}`

```json
{
  "id": "1",
  "name": "Moonveil",
  "category": "weapons",
  "subcategory": "Katana",
  "image": "/weapons/moonveil.png",
  "description": "Katana with a beautifully patterned blade...",
  "lore": "A katana passed down through the Carian royal family...",
  "weight": 6.5,
  
  "attackPower": {
    "physical": 73,
    "magic": 87,
    "fire": 0,
    "lightning": 0,
    "holy": 0,
    "critical": 100
  },
  "guardedDamageNegation": {
    "physical": 31,
    "magic": 57,
    "fire": 26,
    "lightning": 26,
    "holy": 26,
    "guardBoost": 31
  },
  "attributeScaling": {
    "str": "E",
    "dex": "D",
    "int": "C",
    "fai": "-",
    "arc": "-"
  },
  "attributesRequired": {
    "str": 12,
    "dex": 18,
    "int": 23,
    "fai": 0,
    "arc": 0
  },
  "passiveEffects": ["None"],
  "skillName": "Transient Moonlight",
  "skillFpCost": 17,
  
  "location": "Gael Tunnel",
  "howToObtain": "Dropped by Magma Wyrm in Gael Tunnel.",
  "relatedItems": [
    { "id": "2", "name": "Rivers of Blood", "category": "weapons" }
  ]
}
```

#### Armor-specific fields:
```json
{
  "damageNegation": {
    "physical": 13.5,
    "strike": 12.1,
    "slash": 13.8,
    "pierce": 13.5,
    "magic": 10.2,
    "fire": 11.4,
    "lightning": 9.8,
    "holy": 10.5
  },
  "resistance": {
    "immunity": 42,
    "robustness": 67,
    "focus": 28,
    "vitality": 31,
    "poise": 47
  }
}
```

#### Boss-specific fields:
```json
{
  "hp": 33251,
  "defense": 110,
  "stance": 80,
  "difficulty": "Extreme",
  "weaknesses": ["Frostbite", "Bleed"],
  "resistances": ["Scarlet Rot", "Poison"],
  "rewards": ["480,000 Runes", "Remembrance of the Rot Goddess"],
  "drops": [
    { "name": "Malenia's Great Rune", "dropRate": "100%" },
    { "name": "Hand of Malenia", "dropRate": "100%" }
  ],
  "phases": [
    { "name": "Phase 1", "description": "Malenia uses swift katana attacks..." },
    { "name": "Phase 2 - Goddess of Rot", "description": "Transforms with Scarlet Rot wings..." }
  ],
  "strategies": [
    "Use Bloodhound Step to avoid Waterfowl Dance",
    "Summon Spirit Ashes to split aggro"
  ]
}
```

#### Guide-specific fields:
```json
{
  "sections": [
    { "title": "Preparation", "content": "Before fighting Margit..." },
    { "title": "Phase 1", "content": "In the first phase..." }
  ],
  "tips": [
    "Level up to at least 25-30",
    "Use Margit's Shackle item"
  ],
  "videoUrl": "https://youtube.com/watch?v=..."
}
```

#### Location-specific fields:
```json
{
  "enemies": ["Godrick Soldier", "Noble Sorcerer", "Giant"],
  "items": ["Golden Seed", "Smithing Stone [1]"],
  "npcs": ["Merchant Kalé", "Renna"],
  "connectedAreas": ["Stormhill", "Weeping Peninsula", "Liurnia"]
}
```

#### Magic-specific fields:
```json
{
  "fpCost": 40,
  "slots": 3,
  "effects": [
    "Fires a continuous beam of glintstone energy",
    "Consumes FP rapidly while active"
  ]
}
```

---

## 5. Video Guides DTOs

### VideoGuideDTO
**Endpoint:** `GET /api/guides/YouTubeGuides/youtube-guides`

```json
{
  "id": "video-uuid",
  "title": "How to Beat Malenia - Complete Guide",
  "description": "The ultimate guide to defeating Malenia, Blade of Miquella...",
  "thumbnailUrl": "/videos/malenia_guide.jpg",
  "videoUrl": "https://youtube.com/watch?v=example1",
  "duration": "25:43",
  "category": "boss",
  "game": "elden-ring",
  "author": "VaatiVidya",
  "authorAvatar": "/avatars/vaatividya.jpg",
  "views": 2450000,
  "likes": 125000,
  "publishedAt": "2024-03-15",
  "tags": ["malenia", "boss guide", "hard boss"]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | string | Yes | Unique identifier |
| title | string | Yes | Video title |
| description | string | Yes | Video description |
| thumbnailUrl | string | Yes | Thumbnail image URL |
| videoUrl | string | Yes | YouTube/video URL |
| duration | string | Yes | Video duration (format: "MM:SS" or "H:MM:SS") |
| category | string | Yes | "boss", "build", "location", "lore", "pvp", "tips", "walkthrough" |
| game | string | Yes | "dark-souls", "dark-souls-2", "dark-souls-3", "elden-ring", "bloodborne", "sekiro" |
| author | string | Yes | Content creator name |
| authorAvatar | string | No | Author's avatar image |
| views | number | Yes | View count |
| likes | number | Yes | Like count |
| publishedAt | string | Yes | Publication date (ISO 8601 or YYYY-MM-DD) |
| tags | string[] | Yes | Array of tags for filtering |

---

## 6. Gallery/Archive DTOs

### GalleryImageDTO
**Endpoint:** `GET /api/archive/images`

```json
{
  "id": "image-uuid",
  "title": "Firelink Shrine at Dusk",
  "description": "The eternal bonfire of Firelink Shrine...",
  "imageUrl": "/gallery/firelink_shrine.jpg",
  "thumbnailUrl": "/gallery/thumbnails/firelink_shrine.jpg",
  "category": "screenshot",
  "game": "dark-souls",
  "tags": ["firelink", "bonfire", "lordran"],
  "author": "Chosen Undead",
  "likes": 1247,
  "views": 5832,
  "uploadedAt": "2024-01-15"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | string | Yes | Unique identifier |
| title | string | Yes | Image title |
| description | string | No | Image description |
| imageUrl | string | Yes | Full-size image URL |
| thumbnailUrl | string | No | Thumbnail URL (uses imageUrl if not provided) |
| category | string | Yes | "screenshot", "artwork", "wallpaper", "concept", "community" |
| game | string | Yes | Same as VideoGuideDTO |
| tags | string[] | Yes | Array of tags |
| author | string | No | Image author/uploader |
| likes | number | Yes | Like count |
| views | number | Yes | View count |
| uploadedAt | string | Yes | Upload date (ISO 8601 or YYYY-MM-DD) |

---

## 7. API Endpoints Summary

### Authentication (`/api/user/Auth/`)
| Method | Endpoint | Request DTO | Response DTO |
|--------|----------|-------------|--------------|
| POST | `/login` | LoginRequestDTO | AuthResponseDTO |
| POST | `/register` | RegisterRequestDTO | AuthResponseDTO |
| POST | `/refresh-token` | RefreshTokenRequestDTO | AuthResponseDTO |
| POST | `/logout` | - | { success: boolean } |

### User Account (`/api/user/Account/`)
| Method | Endpoint | Request DTO | Response DTO |
|--------|----------|-------------|--------------|
| GET | `/profile` | - (JWT in header) | UserProfileDTO |
| PUT | `/profile` | UpdateUserRequestDTO | UserProfileDTO |

### Builds (`/api/builds/`)
| Method | Endpoint | Request DTO | Response DTO |
|--------|----------|-------------|--------------|
| GET | `/` | - | BuildDataDTO[] |
| GET | `/{id}` | - | BuildDataDTO |
| POST | `/` | BuildDataDTO | BuildDataDTO |
| PUT | `/{id}` | BuildDataDTO | BuildDataDTO |
| DELETE | `/{id}` | - | { success: boolean } |

### Wiki (`/api/wiki/`)
| Method | Endpoint | Request DTO | Response DTO |
|--------|----------|-------------|--------------|
| GET | `/items` | - | WikiItemDTO[] |
| GET | `/items/{category}/{id}` | - | WikiItemDetailDTO |

### Video Guides (`/api/guides/`)
| Method | Endpoint | Request DTO | Response DTO |
|--------|----------|-------------|--------------|
| GET | `/YouTubeGuides/youtube-guides` | - | VideoGuideDTO[] |

### Gallery/Archive (`/api/archive/`)
| Method | Endpoint | Request DTO | Response DTO |
|--------|----------|-------------|--------------|
| GET | `/images` | - | GalleryImageDTO[] |

---

## Notes for Backend Developer

1. **Authentication**: All endpoints except `/Auth/login` and `/Auth/register` require JWT Bearer token in Authorization header.

2. **Date Formats**: Use ISO 8601 format (`2024-03-15T10:30:00Z`) or simple date (`2024-03-15`).

3. **Null Handling**: Fields marked as optional can be omitted or set to null.

4. **Category Values**: Ensure category and game values match exactly as specified (lowercase with hyphens).

5. **Image Paths**: Can be relative paths (served from your static files) or full URLs.

6. **Pagination**: Consider adding pagination support for list endpoints:
   ```json
   {
     "items": [...],
     "totalCount": 150,
     "page": 1,
     "pageSize": 12,
     "totalPages": 13
   }
   ```

7. **Error Responses**: Standardize error responses:
   ```json
   {
     "isSuccess": false,
     "errorMessage": "Detailed error message",
     "errorCode": "VALIDATION_ERROR"
   }
   ```

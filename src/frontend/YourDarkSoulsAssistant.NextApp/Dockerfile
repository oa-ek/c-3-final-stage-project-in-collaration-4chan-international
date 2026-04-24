# СТАДІЯ 1: Збірка (Builder)
FROM node:20-alpine AS builder

# Alpine іноді потребує цієї бібліотеки для роботи певних Node-пакетів
RUN apk add --no-cache libc6-compat

WORKDIR /app

# Копіюємо файли залежностей (package.json та package-lock.json, якщо є)
COPY package.json package-lock.json* ./

# Встановлюємо всі залежності
RUN npm ci || npm install

# Копіюємо весь інший вихідний код фронтенду
COPY . .

# Збираємо Next.js додаток
RUN npm run build

# СТАДІЯ 2: Запуск (Runner)
FROM node:20-alpine AS runner
WORKDIR /app

ENV NODE_ENV production

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

# Копіюємо файли конфігурації (КРИТИЧНО для rewrites)
COPY --from=builder /app/next.config.mjs ./
COPY --from=builder /app/public ./public
COPY --from=builder --chown=nextjs:nodejs /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

USER nextjs
EXPOSE 3000

CMD ["npm", "start"]
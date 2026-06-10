# Deployment

Production deployment runs on a VPS with Docker Compose and Caddy.

## Target

- Domain: `77-239-101-72.sslip.io`
- Server IP: `77.239.101.72`
- Deploy directory: `/opt/genesys-forge`
- Public app URL: `https://77-239-101-72.sslip.io`
- API prefix: `https://77-239-101-72.sslip.io/api`

## VPS bootstrap

Run these commands once on the VPS as `root`.

```bash
lsb_release -a || cat /etc/os-release
docker --version || true
docker compose version || true
```

If Docker is missing:

```bash
apt-get update
apt-get install -y ca-certificates curl gnupg
install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
chmod a+r /etc/apt/keyrings/docker.asc

. /etc/os-release
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu ${VERSION_CODENAME} stable" > /etc/apt/sources.list.d/docker.list

apt-get update
apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
systemctl enable --now docker
```

Open HTTP and HTTPS:

```bash
ufw allow OpenSSH || true
ufw allow 80/tcp
ufw allow 443/tcp
ufw status
```

Create the deploy directory:

```bash
mkdir -p /opt/genesys-forge
```

For HTTPS, the domain must resolve to the VPS IP, and ports `80` and `443` must be reachable from the internet. Caddy requests and renews Let's Encrypt certificates automatically.

## SSH key for GitHub Actions

Create a deploy key locally:

```bash
ssh-keygen -t ed25519 -C "genesys-forge-deploy" -f ~/.ssh/genesys_forge_deploy_key
```

Add the public key to the VPS:

```bash
ssh-copy-id -i ~/.ssh/genesys_forge_deploy_key.pub root@77.239.101.72
```

Then add the private key contents to GitHub Secrets as `VPS_SSH_KEY`.

Do not commit either key file.

After key-based access works, rotate the temporary root password used for the initial setup.

## GitHub Secrets

Repository settings -> Secrets and variables -> Actions -> New repository secret:

| Secret | Value |
| --- | --- |
| `VPS_HOST` | `77.239.101.72` |
| `VPS_PORT` | `22` |
| `VPS_USER` | `root` |
| `VPS_SSH_KEY` | private deploy key contents |
| `APP_DOMAIN` | `77-239-101-72.sslip.io` |
| `ACME_EMAIL` | email for Let's Encrypt notices |
| `GHCR_USERNAME` | GitHub username or deploy user |
| `GHCR_TOKEN` | GitHub PAT with package read access |
| `POSTGRES_DB` | `genesys_forge` |
| `POSTGRES_USER` | `genesys_forge` |
| `POSTGRES_PASSWORD` | strong database password |

`GHCR_TOKEN` is needed if GHCR packages are private. If packages are public, it can be omitted.

## Local Docker check

```bash
docker build -f server/src/GenesysForge.WebApi/Dockerfile -t genesys-forge-webapi .
docker build -f client/Dockerfile -t genesys-forge-client .
```

For production compose, images are pulled from GHCR by GitHub Actions.

## Database access from local machine

PostgreSQL is published only on the VPS loopback interface:

```yaml
127.0.0.1:5432:5432
```

Use an SSH tunnel from your local machine:

```powershell
ssh -L 5433:127.0.0.1:5432 root@77.239.101.72
```

Then connect your database client to:

| Field | Value |
| --- | --- |
| Host | `localhost` |
| Port | `5433` |
| Database | `genesys_forge` |
| User | `genesys_forge` |
| Password | `POSTGRES_PASSWORD` secret value |

## Deployment flow

On push to `master`, GitHub Actions:

1. Builds and tests backend.
2. Lints, tests, and builds frontend.
3. Builds and pushes Docker images to GHCR.
4. Copies `docker-compose.yml` and `Caddyfile` to `/opt/genesys-forge`.
5. Runs `docker compose pull && docker compose up -d --remove-orphans` on the VPS.

## Manual VPS checks

```bash
cd /opt/genesys-forge
docker compose ps
docker compose logs -f caddy
docker compose logs -f webapi
docker compose logs -f postgres
curl -I https://77-239-101-72.sslip.io
curl https://77-239-101-72.sslip.io/api/health
```

If PostgreSQL was started once with an invalid PostgreSQL 18 data mount before the first successful deployment, remove the empty volume and deploy again:

```bash
cd /opt/genesys-forge
docker compose down
docker volume rm genesysforge_postgres_data
docker compose up -d
```

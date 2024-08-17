export type MetagraphInfo = {
    MetagraphName: string;
    MetagraphAddress: string;
    Description: string;
    Website: string;
    CurrentPrice: number;
};

export async function getMetagraphs() {
    const res = await fetch('https://api.nebula-tech.io/dag/v1/metagraphs');

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const metagraphs = data as MetagraphInfo[];
    return metagraphs;
}

import { Metagraph, ValidatorNode } from "@/lib/shared/types";
 
export async function getMetagraphs(network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet'; 

    const res = await fetch(`${apiUrl}/metagraphs/${networkUrl}`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const metagraphs = data as Metagraph[];
    return metagraphs;
}

export async function getHypergraphValidatorNodes(network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet'; 

    const res = await fetch(`${apiUrl}/hypergraph/${networkUrl}/validators`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const validatorNodes = data as ValidatorNode[];
    return validatorNodes;
}

export async function getMetagraphValidatorNodes(network?: string, metagraphAddress?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet'; 

    const url = metagraphAddress
        ? `${apiUrl}/hypergraph/${networkUrl}/validators?metagraphAddress=${metagraphAddress}`
        : `${apiUrl}/hypergraph/${networkUrl}/validators`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const validatorNodes = data as ValidatorNode[];
    return validatorNodes;
}

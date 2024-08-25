import { ValidatorNode } from "@/lib/shared/types";

 
export async function getHypergraphValidatorNodes() {
    const res = await fetch('https://localhost:60016/hypergraph/mainnet/validators');

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const validatorNodes = data as ValidatorNode[];
    return validatorNodes;
}

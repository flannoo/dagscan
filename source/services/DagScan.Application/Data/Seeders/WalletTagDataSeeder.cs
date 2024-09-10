using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;

namespace DagScan.Application.Data.Seeders;

public sealed class WalletTagDataSeeder(
    DagContext dagContext)
    : IDataSeeder
{
    public int Order => 4;

    public async Task SeedAsync()
    {
        if (dagContext.WalletTags.Any())
        {
            return;
        }

        var walletTags = new List<WalletTag>
        {
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6LvxLSdWoC9uJZPgXtcmkcWBaGYypF6smaPyH"),
                Tag = "BitForex Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG3Lcv4GEhPH34VHVgbEAf21Y3L2rtjLpXh7QD4"),
                Tag = "CoinEx Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8BSw4tGn269vbF9wor4FGd4yt9XGdJuiMb6uS"),
                Tag = "Data Pool Distribution"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG3RXBWBJq1Bf38rawASakLHKYMbRhsDckaGvGu"),
                Tag = "Data Reward Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8s4uKsTKV5hNVv9oHWophX1CYKVqJ88hM9MZE"),
                Tag = "DTM Enterprise Client"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG06pFXdTtqrx2H11oHyH5rBe6Ccx7XG8WSsPSA"),
                Tag = "DTM Enterprise Client"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5hKjytwENhcx2vmC5qWyWSgnqWmYq7cUvGLDG"),
                Tag = "DTM Reward Distribution"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0Njmo6JZ3FhkLsipJSppepUHPuTXcSifARfvK"),
                Tag = "DTM Reward Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG4TETUwraLYX1mYdC8ymUxxWsoNZPffUpDf4Ar"), Tag = "Gate Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5LEmVdtMGryBs1UBLMfg37LoSiVHC8Q4odps9"),
                Tag = "Integrationnet Reward Distribution"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8jE4CHy9T2izWFEv8K6rp5hNJq11SyLEVYnt8"),
                Tag = "Integrationnet Reward Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG2Evedeb9cS7d28bxF4wwgeryiEqfDo8diZMZg"),
                Tag = "Kucoin Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG2rMPHX4w1cMMjowmewRMjD1in53yRURt6Eijh"),
                Tag = "Kucoin Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5yqn4JRkW5oAMthhBayBtkZzfAvRQnkH1dCG4"),
                Tag = "Kucoin Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6cStT1VYZdUhpoME23U5zbTveYq78tj7EihFV"),
                Tag = "Kucoin Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG4nBMH7KwFAnpfB6VaFRaKEwACNSidzeMuPtgV"),
                Tag = "Lattice Treasury"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6Yxge8Tzd8DJDJeL4hMLntnhheHGR4DYSPQvf"), Tag = "MEXC Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6stSuh8C46VVHvFybyCjdC4UPu42xXq2Si9Z3"),
                Tag = "Softnode Reward Distribution"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG77VVVRvdZiYxZ2hCtkHz68h85ApT5b2xzdTkn"),
                Tag = "Softnode Reward Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6qyCvhka9rX9SsAMouHmAoKmADuGW415anB59"),
                Tag = "Stardust Collective (team controlled)"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8VT7bxjs1XXBAzJGYJDaeyNxuThikHeUTp9XY"),
                Tag = "Stardust Collective"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8vD8BUhCpTnYXEadQVGhHjgxEZZiafbzwmKKh"),
                Tag = "Stardust Collective"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAGSTARDUSTCOLLECTIVEHZOIPHXZUBFGNXWJETZVSPAPAHMLXS"),
                Tag = "Stardust Collective"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG07znCvSyM2xhxPZECrGhVF6WVPMvFWe6Z6EWW"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0kib4L4YYZCWrch6eBYeQkYCywTCuEZX58vTp"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG1ZieMRm7ALEbSjmvwztvtZYu7srPaXwxbC14U"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG3tC21XtXvoUD8hTMQzHm7T21MHahuFPVrPBtR"), Tag = "Treasury"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG86Joz5S7hkL8N9yqTuVs5vo1bzQLwF3MUTUMX"), Tag = "Treasury"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG1nw5WkZdQf96Df3PkrjLxeHj2EV3oLkWPZQcD"), Tag = "Treasury"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG2eFDjZ2CMA3M4KMfLw6Vnn7kaJPJqcSCpHU25"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG2ttEXvYHsMP5qu7ejoBTbuCPmHoDhU5fZi3YL"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG38whfr5CWzMoQg8PajuiukNNojySqyXtZdBhK"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG3Sycgyv3UHQM3JDuGguoF5776ceR8GiwpdFJ9"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG3yzY9252n8Fkxix7pZo5TH6F9paxSVLsDARK4"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6ipRGeHmgGNNsPG8nQG4txeFNxwXJnXCEn3zQ"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG7teqwiZjuBivJi7Mx8AkhwnF6w3Q1poUTCViK"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG7uFTujXArFTuTqELGYGcthacpfQykBX7wsgFv"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8MWCDLPxjufRE2tkg3qpWSd7iJKFfsg9H5nCE"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG8UsoSR14peffVJKAsf3mqJFnkKSoQEUQDAQKN"),
                Tag = "Team Foundation"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0UfhrKrsfVJCquFQmGc6U3ZsQTZfYTi9MHdAv"),
                Tag = "Team Validator Node"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG1WwnSNfVDmVNk53Unj4fZSW49c2VfHs1vBPAX"),
                Tag = "Team Validator Node"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG1hQvNovVmyqAYUi72G1JjfxupwVXWwshgnUxZ"),
                Tag = "Team Validator Node"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0qE5tkz6cMUD5M2dkqgfV4TQCzUUdAP5MFM9P"),
                Tag = "Testnet Reward Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG1pLpkyX7aTtFZtbF98kgA9QTZRzrsGaFmf4BT"),
                Tag = "Uphold Exchange"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0o6WSyvc7XfzujwJB1e25mfyzgXoLYDD6wqnk"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG4YD6rkExLwYyAZzwjYJMxe36PAptKuUKq9uc7"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG4nBD5J3Pr2uHgtS1sa16PqemHrwCcvjdR31Xe"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6B5mBMoEu3Habtb2ts3QGUD2UquywrQSLSubU"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5uDuGhPuh4mQZGNLFCEcdy69txSF4iSfFbdWJ"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5fqiGq9L5iLH5R5eV7gBjkucewrcaQ1jVnKYD"), Tag = "DOR Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG045Bmio7Jrv3aErTKjAisRnpBKvp16pp1wSqT"),
                Tag = "DOR Validator Tax Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG2JsH1QKj8LrzmcgX2pf9MAcdhQWuihYnZMUNW"),
                Tag = "DOR Validator Tax Distribution"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG2uFJWUMHW8pZXXeaUMtUpWwZvhpr3BBBtBYEp"),
                Tag = "Commercial License Fee"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG0U7R9jXMSiNMU5mgqpvCVuaBwfRBzY77nJZM1"),
                Tag = "DTM Reward Distribution Pool"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG834Eqd26HWuaqLFew4p7bxXdAnAYMV1B8ZxfM"),
                Tag = "PylonFi DOR Node #1"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5LWn8fQi8acadBD4BuWuuzrHLnxDWcD3onmcY"),
                Tag = "PylonFi DOR Node #2"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG39RKLvMfUkA2UgN5XDmsKUm2X8GJ3drbTg1XX"),
                Tag = "El Paca Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG5VxUBiDx24wZgBwjJ1FeuVP1HHVjz6EzXa3z6"),
                Tag = "El Paca Metagraph"
            },
            new WalletTag
            {
                WalletAddress = new WalletAddress("DAG6PEUki8A8bewE2QqGWQ7o9ZwN8j4V57pUn2Lt"),
                Tag = "El Paca Metagraph"
            },
        };

        dagContext.WalletTags.AddRange(walletTags);
        await dagContext.SaveChangesAsync();
    }
}

mergeInto(LibraryManager.library, {
  // Funkcja do wysy³ania tokenów ERC-20 (np. USDT)
  SendERC20Transaction: function (privateKey, recipientAddress, amountInToken, contractAddress, callback, rpc) {
    // Upewnij siê, ¿e biblioteka Web3.js jest za³adowana
    if (typeof Web3 === 'undefined') {
      console.error('Web3 is not defined');
      return;
    }

    const whatRPC = UTF8ToString(rpc);
    // Zainicjalizuj web3 dla sieci Polygon (lub innej sieci obs³uguj¹cej ERC-20)
    const web3 = new Web3(whatRPC); // RPC URL for Polygon or Ethereum Mainnet

    // Konwersja parametrów
    const privateKeyString = UTF8ToString(privateKey);
    const recipientAddressString = UTF8ToString(recipientAddress);
    const amountInTokenString = UTF8ToString(amountInToken); // Kwota w tokenach
    const contractAddressString = UTF8ToString(contractAddress); // Adres kontraktu ERC-20
       const amountInBigNumber = new BigNumber(amountInTokenString);
   // console.log("Contract Address: ", contractAddressString);

    // Sprawdzenie, czy adres odbiorcy jest poprawny
    if (!web3.utils.isAddress(recipientAddressString)) {
      console.error("Invalid Ethereum address: ", recipientAddressString);
      return;
    }

    // Sprawdzenie, czy adres kontraktu jest poprawny
    if (!web3.utils.isAddress(contractAddressString)) {
      console.error("Invalid contract address: ", contractAddressString);
      return;
    }

    // Konwersja kwoty tokenów do najmniejszych jednostek

    const account = web3.eth.accounts.privateKeyToAccount(privateKeyString);
    web3.eth.accounts.wallet.add(account);

    // ABI standardowego kontraktu ERC-20
    const contractAbi = [
      {
        'constant': false,
        'inputs': [
          { 'name': '_to', 'type': 'address' },
          { 'name': '_value', 'type': 'uint256' }
        ],
        'name': 'transfer',
        'outputs': [
          { 'name': '', 'type': 'bool' }
        ],
        'type': 'function'
      }
    ];

    const contract = new web3.eth.Contract(contractAbi, contractAddressString);

    // Pobierz aktualny nonce
    web3.eth.getTransactionCount(account.address, 'pending')
      .then(nonce => {
        // Stwórz transakcjê ERC-20 (transfer tokenów)
        contract.methods.transfer(recipientAddressString, amountInBigNumber.toString())
          .send({
            from: account.address,
            gas: 90000, // Limit gazu
            gasPrice: web3.utils.toWei('90', 'gwei'), // Cena gazu
            nonce: nonce // Aktualny nonce z sieci
          })
          .then(function (receipt) {
            try {
              console.log("Transaction success, hash: ", receipt.transactionHash);
              SendMessage("PlayFabObject", "OnTransactionComplete", receipt.transactionHash);
            } catch (e) {
              console.error('Callback error: ', e);
            }
          })
          .catch(function (error) {
            console.error('Transaction failed', error);
          });
      })
      .catch(function (error) {
        console.error('Failed to get nonce', error);
      });
  },

  // Funkcja do wysy³ania tokenów natywnych (np. MATIC, ETH)
  SendNativeTransaction: function (privateKey, recipientAddress, amountInEther, callback, rpc) {
    // Upewnij siê, ¿e biblioteka Web3.js jest za³adowana
    if (typeof Web3 === 'undefined') {
      console.error('Web3 is not defined');
      return;
    }
        const whatRPC = UTF8ToString(rpc);

    // Zainicjalizuj web3 dla sieci Polygon (lub innej sieci obs³uguj¹cej natywne tokeny)
    const web3 = new Web3(whatRPC); // RPC URL for Polygon or Ethereum Mainnet

    // Konwersja parametrów
    const privateKeyString = UTF8ToString(privateKey);
    const recipientAddressString = UTF8ToString(recipientAddress);
    const amountInEtherString = UTF8ToString(amountInEther); // Kwota w MATIC lub ETH


    // Sprawdzenie, czy adres odbiorcy jest poprawny
    if (!web3.utils.isAddress(recipientAddressString)) {
      console.error("Invalid Ethereum address: ", recipientAddressString);
      return;
    }

    // Konwersja kwoty MATIC/ETH do najmniejszych jednostek (Wei)
    const amountInWei = web3.utils.toWei(amountInEtherString, 'ether'); // Konwersja na Wei

    const account = web3.eth.accounts.privateKeyToAccount(privateKeyString);
    web3.eth.accounts.wallet.add(account);

    // Pobierz aktualny nonce
    web3.eth.getTransactionCount(account.address, 'pending')
      .then(nonce => {
        // Stwórz transakcjê natywn¹
        const tx = {
          from: account.address,
          to: recipientAddressString,
          value: amountInWei, // Kwota w Wei
          gas: 21000, // Standardowy limit gazu dla prostego transferu
          gasPrice: web3.utils.toWei('90', 'gwei'), // Cena gazu
          nonce: nonce // Aktualny nonce z sieci
        };

        // Wys³anie transakcji
        web3.eth.sendTransaction(tx)
          .then(function (receipt) {
            try {
              console.log("Transaction success, hash: ", receipt.transactionHash);
              SendMessage("PlayFabObject", "OnTransactionComplete", receipt.transactionHash);
            } catch (e) {
              console.error('Callback error: ', e);
            }
          })
          .catch(function (error) {
            console.error('Transaction failed', error);
          });
      })
      .catch(function (error) {
        console.error('Failed to get nonce', error);
      });
  }
});

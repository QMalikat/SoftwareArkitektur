function update()
{
    el = document.getElementById("data");
    data = document.getElementById("input").value;
    
    el.innerHTML = data;
    
    fetch('http://localhost:5033/hello', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: data })
    });


    fetch("http://localhost:5033/hello")
    .then(response => {
        if (!response.ok) {
            throw new Error("Network response was not ok " + response.statusText);
        }
        return response.text();
    })
    .then(data => {
        console.log(data);
        el.innerHTML = data;
    });
}

// Function to update the "hello" message with a new name
async function updateHello() {
    const name = inputField.value.trim();
    if (!name) {
        alert('Please enter a name!');
        return;
    }

    try {

    } catch (error) {
        console.error('Failed to update hello message:', error);
        alert('Failed to update greeting.');
    }
}
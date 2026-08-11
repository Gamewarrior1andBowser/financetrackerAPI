const $ = id => document.getElementById(id);

const formatCurrency = amount => Number(amount || 0).toLocaleString("en-US", {
    style: "currency",
    currency: "USD"
});

async function loadReports() {
    try {
        const response = await fetch("/Reports/GetReportData");
        if (!response.ok) throw new Error("Could not load report data.");

        const data = await response.json();
        const income = Number(data.income) || 0;
        const expenses = Number(data.expenses) || 0;
        const categories = data.categories || [];

        $("totalIncome").textContent = formatCurrency(income);
        $("totalExpenses").textContent = formatCurrency(expenses);
        $("remainingBalance").textContent = formatCurrency(income - expenses);

        new Chart($("incomeExpenseChart"), {
            type: "bar",
            data: {
                labels: ["Income", "Expenses"],
                datasets: [{
                    data: [income, expenses],
                    backgroundColor: ["#5965c9", "#e36b73"],
                    borderRadius: 7,
                    borderSkipped: false,
                    barThickness: 55
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        callbacks: {
                            label: ({ raw }) => formatCurrency(raw)
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: "#eeeef3" },
                        border: { display: false },
                        ticks: {
                            color: "#888894",
                            callback: value => "$" + value.toLocaleString()
                        }
                    },
                    x: {
                        grid: { display: false },
                        border: { display: false },
                        ticks: { color: "#666675" }
                    }
                }
            }
        });

        const colors = [
            "#5965c9",
            "#7a84d8",
            "#9ca4e4",
            "#b9bfec",
            "#d4d7f3",
            "#e36b73",
            "#e9979d"
        ];

        new Chart($("categoryChart"), {
            type: "doughnut",
            data: {
                labels: categories.map(x => x.category),
                datasets: [{
                    data: categories.map(x => Number(x.amount) || 0),
                    backgroundColor: colors,
                    borderWidth: 3,
                    borderColor: "#fff"
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: "68%",
                plugins: {
                    legend: { display: false }
                }
            }
        });

        const list = $("categoryList");
        list.innerHTML = "";

        categories.forEach((item, i) => {
            const row = document.createElement("div");
            row.className = "category-item";

            const name = document.createElement("span");
            name.className = "category-name";

            const dot = document.createElement("span");
            dot.className = "category-dot";
            dot.style.background = colors[i % colors.length];

            name.append(dot, document.createTextNode(item.category));

            const amount = document.createElement("span");
            amount.className = "category-amount";
            amount.textContent = formatCurrency(item.amount);

            row.append(name, amount);
            list.appendChild(row);
        });
    } catch (error) {
        console.error("Error loading reports:", error);
    }
}

loadReports();


import {LitElement, html, css} from 'lit';

export class ToDoList extends LitElement {
  static properties = {
    _listItems: {state: true},
    hideCompleted: {},
  };
  static styles = css`
    .completed {
      text-decoration-line: line-through;
      color: #777;
    }`;

  constructor() {
    super();
    this._listItems = [
      {text: 'Make to-do list', completed: true},
      {text: 'Complete Lit tutorial', completed: false},
    ];
    this.hideCompleted = false;
  }

  async firstUpdated() {
	this.getItems();
  }

  render() {
    const items = this.hideCompleted
        ? this._listItems.filter((item) => !item.isComplete)
        : this._listItems;
    const todos = html`
      <ul>
        ${items.map(
          (item) => html`
            <li
                class=${item.isComplete ? 'completed' : ''}
                @click=${() => this.toggleCompleted(item)}>
              ${item.name}
            </li>`
        )}
      </ul>
    `;
    const caughtUpMessage = html`
        <p>
        You're all caught up!
        </p>
    `;
    const todosOrMessage = items.length > 0
        ? todos
        : caughtUpMessage;
    return html`
      <h2>To Do</h2>
      <!-- TODO: Update expression. -->
      ${todosOrMessage}
      <input id="newitem" aria-label="New item">
      <button @click=${this.addToDo}>Add</button>
      <br>
      <label>
        <input type="checkbox"
          @change=${this.setHideCompleted}
          ?checked=${this.hideCompleted}>
        Hide completed
      </label>
    `;
  }

  async toggleCompleted(item) {
	console.log(item);
	const response = await fetch(`https://localhost:7233/api/Todo?id=${item.id}`, {
	  method: 'PUT',
	});
	this.getItems();
  }

  setHideCompleted(e) {
    this.hideCompleted = e.target.checked;
  }

  get input() {
    return this.renderRoot?.querySelector('#newitem') ?? null;
  }

  async addToDo() {
	const input = this.input;
	if (input && input.value) {
	  const response = await fetch('https://localhost:7233/api/Todo', {
		method: 'POST',
		headers: {
		  'Content-Type': 'application/json',
		},
		body: JSON.stringify({name: input.value}),
	  });
	  this.getItems();
	  input.value = '';
	}
  }

  async getItems() {
	const response = await fetch('https://localhost:7233/api/Todo');
	const data = await response.json();
	this._listItems = data;
  }

}
customElements.define('todo-list', ToDoList);

import type {OnInit} from '@angular/core';
import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Allergen} from "../tab2.page";

@Component({
  selector: 'app-typeahead',
  templateUrl: 'typeahead.component.html',
})
export class TypeaheadComponent implements OnInit {
  @Input() items: Allergen[] = [];
  @Input() selectedItems: number[] = [];

  @Output() selectionCancel = new EventEmitter<void>();
  @Output() selectionChange = new EventEmitter<number[]>();

  filteredItems: Allergen[] = [];
  workingSelectedValues: number[] = [];

  ngOnInit() {
    this.filteredItems = [...this.items];
    this.workingSelectedValues = [...this.selectedItems];
  }

  trackItems(index: number, item: Allergen) {
    return item.category_name;
  }

  cancelChanges() {
    this.selectionCancel.emit();
  }

  confirmChanges() {
    this.selectionChange.emit(this.workingSelectedValues);
  }

  // @ts-ignore
  searchbarInput(ev) {
    this.filterList(ev.target.value);
  }

  /**
   * Update the rendered view with
   * the provided search query. If no
   * query is provided, all data
   * will be rendered.
   */
  filterList(searchQuery: string | undefined) {
    /**
     * If no search query is defined,
     * return all options.
     */
    if (searchQuery === undefined) {
      this.filteredItems = [...this.items];
    } else {
      /**
       * Otherwise, normalize the search
       * query and check to see which items
       * contain the search query as a substring.
       */
      const normalizedQuery = searchQuery.toLowerCase();
      this.filteredItems = this.items.filter((item) => {
        return item.category_name.toLowerCase().includes(normalizedQuery);
      });
    }
  }

  isChecked(value: number) {
    return this.workingSelectedValues.find((item) => item === value);
  }

  // @ts-ignore
  checkboxChange(ev) {
    const { checked, value } = ev.detail;

    if (checked) {
      this.workingSelectedValues = [...this.workingSelectedValues, value];
    } else {
      this.workingSelectedValues = this.workingSelectedValues.filter((item) => item !== value);
    }
  }
}
